import React, { useEffect, useState, useRef } from "react";
import {
  Box, Paper, Typography, Container, CircularProgress, Divider,
  List, ListItem, ListItemText, TextField, Button
} from "@mui/material";
import { getCardInfoApi, getHistoryApi } from "../api/paymentApi";
import { getProfileApi } from "../api/userApi";
import * as signalR from "@microsoft/signalr";
import { getDashboardApi } from "../api/dashboardApi";
import DashboardCard from "../components/DashboardCard";
import AccountBalanceWalletIcon from "@mui/icons-material/AccountBalanceWallet";
import CreditCardIcon from "@mui/icons-material/CreditCard";
import ReceiptLongIcon from "@mui/icons-material/ReceiptLong";
import SavingsIcon from "@mui/icons-material/Savings";
import RestaurantIcon from "@mui/icons-material/Restaurant";
import DirectionsCarIcon from "@mui/icons-material/DirectionsCar";
import ShoppingCartIcon from "@mui/icons-material/ShoppingCart";
import MovieIcon from "@mui/icons-material/Movie";
import CategoryIcon from "@mui/icons-material/Category";
import SpendingCharts from "../components/SpendingCharts";

interface ProfileData { id: string; email: string; fullName: string; }
interface CardInfo { brand: string; last4: string; expMonth: number; expYear: number; }
interface Transaction {
  id: string;
  amount: number;
  status: string;
  date: string;
  card: string;
  description: string,
  category: string;
}
interface AIMessage { user: string; message: string; }
interface DashboardData {
  balance: number;
  spentThisMonth: number;
  transactions: number;
  cards: number;
  monthlySpending: {
    month: string;
    amount: number;
  }[];
  categorySpending: {
    category: string;
    amount: number;
  }[];
}

const ProfilePage: React.FC = () => {
  const [profile, setProfile] = useState<ProfileData | null>(null);
  const [cards, setCards] = useState<CardInfo[]>([]);
  const [history, setHistory] = useState<Transaction[]>([]);
  const [dashboard, setDashboard] = useState<DashboardData | null>(null);
  const [loading, setLoading] = useState(true);

  const [aiMessages, setAiMessages] = useState<AIMessage[]>([]);
  const [input, setInput] = useState("");
  const [connected, setConnected] = useState(false);

  const connectionRef = useRef<signalR.HubConnection | null>(null);
  const messagesEndRef = useRef<HTMLDivElement | null>(null);

  const getCategoryIcon = (category: string) => {
    switch (category) {
      case "Food":
        return <RestaurantIcon />;

      case "Transport":
        return <DirectionsCarIcon />;

      case "Shopping":
        return <ShoppingCartIcon />;

      case "Entertainment":
        return <MovieIcon />;

      default:
        return <CategoryIcon />;
    }
  };

  useEffect(() => {
    const loadData = async () => {
      try {
        const [p, c, h, d] = await Promise.all([
          getProfileApi(),
          getCardInfoApi(),
          getHistoryApi(),
          getDashboardApi()
        ]);

        setProfile(p);
        setCards(c);
        setHistory(h);
        setDashboard(d);

      } catch (err) {
        console.error("Unauthorized", err);
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, []);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7014/chatHub", {
        accessTokenFactory: () => localStorage.getItem("token") ?? ""
      })
      .withAutomaticReconnect()
      .build();

    connection.on("ReceiveMessage", (user: string, message: string) => {
      setAiMessages(prev => [...prev, { user, message }]);
    });

    const startConnection = async () => {
      try {
        await connection.start();
        setConnected(true);
        console.log("SignalR Connected");
      } catch (err) {
        console.error("SignalR Connection Error:", err);
      }
    };

    startConnection();

    connection.onclose(() => {
      setConnected(false);
      console.log("SignalR Disconnected");
    });

    connectionRef.current = connection;

    return () => {
      connection.stop();
    };

  }, []);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [aiMessages]);

  const sendAiMessage = async () => {

    if (!input.trim()) return;

    if (!connectionRef.current) return;

    if (connectionRef.current.state !== signalR.HubConnectionState.Connected) {
      console.log("SignalR not connected");
      return;
    }

    try {

      await connectionRef.current.invoke(
        "SendMessage",
        input
      );

      setInput("");

    } catch (err) {
      console.error("Send message error:", err);
    }
  };

  if (loading) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Container maxWidth={false}>
      <Paper sx={{ p: 4, borderRadius: 3, background: "linear-gradient(135deg,#F7F4F9,#EDE7F2)" }}>
        <Typography variant="h4"
          sx={{
            fontFamily: "Playfair Display, serif",
            color: "#4B3553",
            mb: 2,
            textAlign: "center"
          }}
        >
          Your Profile
        </Typography>
        <Typography sx={{ mb: 1 }}>
          <strong>Name:</strong> {profile?.fullName}
        </Typography>
        <Typography sx={{ mb: 2 }}>
          <strong>Email:</strong> {profile?.email}
        </Typography>
        <Divider sx={{ my: 2 }} />
        <Box
          sx={{
            display: "grid",
            gridTemplateColumns: {
              xs: "1fr",
              sm: "repeat(4,1fr)"
            },
            gap: 2,
            mb: 3
          }}
        >
          <DashboardCard
            title="Balance"
            value={`$${dashboard?.balance ?? 0}`}
            icon={<SavingsIcon />}
          />
          <DashboardCard
            title="Spent this month"
            value={`$${dashboard?.spentThisMonth ?? 0}`}
            icon={<AccountBalanceWalletIcon />}
          />
          <DashboardCard
            title="Transactions"
            value={dashboard?.transactions ?? 0}
            icon={<ReceiptLongIcon />}
          />
          <DashboardCard
            title="Cards"
            value={dashboard?.cards ?? 0}
            icon={<CreditCardIcon />}
          />
        </Box>
        {
          dashboard && (
            <SpendingCharts
              monthlySpending={dashboard.monthlySpending}
              categorySpending={dashboard.categorySpending}
            />
          )
        }
        <Divider sx={{ my: 2 }} />
        <Typography variant="h6" sx={{ color: "#4B3553", mb: 1 }}>
          Saved Cards
        </Typography>
        {cards.length === 0 &&
          <Typography color="text.secondary">No cards saved</Typography>
        }
        <List dense>
          {cards.map((card, i) => (
            <ListItem key={i}>
              <ListItemText
                primary={`${card.brand.toUpperCase()} •••• ${card.last4}`}
                secondary={`Exp: ${card.expMonth}/${card.expYear}`}
              />
            </ListItem>
          ))}
        </List>
        <Divider sx={{ my: 2 }} />
        <Typography variant="h6" sx={{ color: "#4B3553", mb: 1 }}>
          Recent Transactions
        </Typography>
        {history.length === 0 &&
          <Typography color="text.secondary">No transactions yet</Typography>
        }
        <List dense>
          {history.slice(0, 5).map((t) => (
            <ListItem key={t.id}>
              <ListItem
                key={t.id}
                secondaryAction={getCategoryIcon(t.category)}
              >
                <ListItemText
                  primary={
                    <>
                      ${t.amount} — {t.status}
                    </>
                  }
                  secondary={
                    <>
                      {t.category} • {t.description}
                      <br />
                      {new Date(t.date).toLocaleString()}
                      {" | "}
                      Card •••• {t.card}
                    </>
                  }
                />
              </ListItem>
            </ListItem>
          ))}
        </List>
        <Divider sx={{ my: 2 }} />
        <Typography variant="h6" sx={{ color: "#4B3553", mb: 1 }}>
          AI Chat
        </Typography>
        <Paper
          sx={{
            p: 2,
            height: 300,
            overflowY: "auto",
            display: "flex",
            flexDirection: "column",
            gap: 1
          }}
        >
          {aiMessages.map((m, i) => (
            <Box
              key={i}
              sx={{
                p: 1.5,
                borderRadius: 2,
                background: m.user === "AI Bot"
                  ? "#D1C4E9"
                  : "#EDE7F2"
              }}
            >
              <Typography sx={{ fontWeight: 600 }}>
                {m.user}
              </Typography>
              <Typography>
                {m.message}
              </Typography>
            </Box>
          ))}
          <div ref={messagesEndRef} />
        </Paper>
        <Box sx={{ display: "flex", gap: 1, mt: 1 }}>
          <TextField
            label="Message"
            value={input}
            onChange={(e) => setInput(e.target.value)}
            sx={{ flex: 1 }}
            onKeyDown={(e) => {
              if (e.key === "Enter") sendAiMessage();
            }}
          />
          <Button
            variant="contained"
            disabled={!connected}
            onClick={sendAiMessage}
          >
            Send
          </Button>
        </Box>
      </Paper>
    </Container>
  );
};

export default ProfilePage;
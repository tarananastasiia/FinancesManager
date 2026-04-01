import React, { useEffect, useState, useRef } from "react";
import {
  Box, Paper, Typography, Container, CircularProgress, Divider,
  List, ListItem, ListItemText, TextField, Button
} from "@mui/material";
import { getCardInfoApi, getHistoryApi } from "../api/paymentApi";
import { getProfileApi } from "../api/userApi";
import * as signalR from "@microsoft/signalr";

interface ProfileData { id: string; email: string; fullName: string; }
interface CardInfo { brand: string; last4: string; expMonth: number; expYear: number; }
interface Transaction { id: string; amount: number; status: string; date: string; card: string; }
interface AIMessage { user: string; message: string; }

const ProfilePage: React.FC = () => {

  const [profile, setProfile] = useState<ProfileData | null>(null);
  const [cards, setCards] = useState<CardInfo[]>([]);
  const [history, setHistory] = useState<Transaction[]>([]);
  const [loading, setLoading] = useState(true);

  const [aiMessages, setAiMessages] = useState<AIMessage[]>([]);
  const [input, setInput] = useState("");
  const [connected, setConnected] = useState(false);

  const connectionRef = useRef<signalR.HubConnection | null>(null);
  const messagesEndRef = useRef<HTMLDivElement | null>(null);

  // Load profile data
  useEffect(() => {
    const loadData = async () => {
      try {
        const [p, c, h] = await Promise.all([
          getProfileApi(),
          getCardInfoApi(),
          getHistoryApi(),
        ]);

        setProfile(p);
        setCards(c);
        setHistory(h);

      } catch (err) {
        console.error("Unauthorized", err);
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, []);

  // SignalR connection
  useEffect(() => {

    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7014/chatHub", {
        withCredentials: true
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

  // Auto scroll chat
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
        "You",
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
          <Typography variant="h6" sx={{ color: "#4B3553", mb: 1 }}>
            💳 Saved Cards
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
            📜 Recent Transactions
          </Typography>
          {history.length === 0 &&
            <Typography color="text.secondary">No transactions yet</Typography>
          }
          <List dense>
            {history.slice(0, 5).map((t) => (
              <ListItem key={t.id}>
                <ListItemText
                  primary={`$${t.amount} — ${t.status}`}
                  secondary={`${new Date(t.date).toLocaleString()} | Card •••• ${t.card}`}
                />
              </ListItem>
            ))}
          </List>
          <Divider sx={{ my: 2 }} />
          <Typography variant="h6" sx={{ color: "#4B3553", mb: 1 }}>
            🤖 AI Chat
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
          <Typography
            sx={{
              mt: 3,
              color: "#7A6A86",
              textAlign: "center"
            }}
          >
            Secure Financial Platform
          </Typography>
        </Paper>
      </Container>
  );
};

export default ProfilePage;
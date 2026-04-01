import React from "react";
import { CardElement, useStripe, useElements } from "@stripe/react-stripe-js";
import { createPaymentIntent } from "../../../api/paymentApi";
import { Box, Button, Container, Paper, Typography, Alert, Divider } from "@mui/material";

interface CheckoutFormProps {
  error?: string;
  loading?: boolean;
}

const CheckoutForm: React.FC<CheckoutFormProps> = ({ error, loading }) => {
  const stripe = useStripe();
  const elements = useElements();

  const pay = async () => {
    if (!stripe || !elements) return;

    const secret = await createPaymentIntent();

    const result = await stripe.confirmCardPayment(secret, {
      payment_method: {
        card: elements.getElement(CardElement)!,
      },
    });

    if (result.error) {
      alert(result.error.message);
    } else {
      alert("Payment successful ✅");
    }
  };

  return (
    <Box
      sx={{
        minHeight: "100vh",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        background: "linear-gradient(145deg, #FAF0F5, #F0E7FA, #EDE2F7)",
        fontFamily: "Playfair Display, serif",
      }}
    >
      <Container maxWidth="sm">
        <Paper
          elevation={8}
          sx={{
            p: 6,
            borderRadius: 4,
            border: "1px solid rgba(190,160,200,0.3)",
            boxShadow: "0 12px 35px rgba(120,80,130,0.25)",
            backgroundColor: "rgba(255, 250, 255, 0.95)",
            position: "relative",
            overflow: "hidden",
          }}
        >
          <Box
            sx={{
              position: "absolute",
              top: -30,
              left: -20,
              width: "120%",
              height: 100,
              background: "radial-gradient(circle at center, #FADFFB 0%, transparent 70%)",
              transform: "rotate(-15deg)",
              opacity: 0.2,
            }}
          />

          <Typography
            variant="h3"
            align="center"
            sx={{
              color: "#4B3553",
              fontWeight: 700,
              mb: 1,
              letterSpacing: 1,
            }}
          >
            Checkout
          </Typography>

          <Typography
            align="center"
            sx={{
              color: "#7A6A86",
              fontSize: "0.95rem",
              mb: 4,
              fontStyle: "italic",
              letterSpacing: "0.5px",
            }}
          >
            Enter your card for a refined financial experience
          </Typography>

          {error && (
            <Alert severity="error" sx={{ mb: 3 }}>
              {error}
            </Alert>
          )}

          <Box
            sx={{
              p: 3,
              border: "1px solid #D8BCE3",
              borderRadius: 3,
              backgroundColor: "#FCF5FF",
              mb: 4,
              transition: "all 0.3s ease",
              "&:hover": {
                borderColor: "#C8A0E0",
                boxShadow: "0 4px 12px rgba(200,160,220,0.3)",
              },
            }}
          >
            <CardElement
              options={{
                style: {
                  base: {
                    fontSize: "16px",
                    color: "#4B3553",
                    fontFamily: "Playfair Display, serif",
                    "::placeholder": { color: "#BFA5D4" },
                  },
                  invalid: { color: "#D32F2F" },
                },
              }}
            />
          </Box>

          <Divider
            sx={{
              my: 3,
              borderColor: "rgba(190,160,200,0.3)",
              "&::before, &::after": {
                borderTop: "1px dotted #C8A0E0",
              },
            }}
          />

          <Button
            type="button"
            fullWidth
            onClick={pay}
            disabled={loading}
            sx={{
              py: 1.6,
              borderRadius: 3,
              fontWeight: 700,
              fontSize: "1rem",
              background: "linear-gradient(135deg, #E4D2A8, #C8B08A)",
              color: "#3E2F1A",
              boxShadow: "0 8px 20px rgba(180,150,80,0.35)",
              transition: "all 0.3s ease",
              "&:hover": {
                background: "linear-gradient(135deg, #D9C28F, #B89E74)",
                boxShadow: "0 10px 25px rgba(180,150,80,0.5)",
                transform: "translateY(-2px)",
              },
            }}
          >
            {loading ? "Processing..." : "Pay $20"}
          </Button>

          <Typography
            sx={{
              mt: 4,
              textAlign: "center",
              fontSize: "0.85rem",
              color: "#7A6A86",
              fontStyle: "italic",
            }}
          >
            Your payment is secure!
          </Typography>
        </Paper>
      </Container>
    </Box>
  );
};

export default CheckoutForm;
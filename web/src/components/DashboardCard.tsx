import React from "react";
import { Paper, Typography } from "@mui/material";

interface DashboardCardProps {
    title: string;
    value: string | number;
    icon?: React.ReactNode;
}

const DashboardCard: React.FC<DashboardCardProps> = ({
    title,
    value,
    icon
}) => {
    return (
        <Paper
            sx={{
                p: 2,
                borderRadius: 3,
                background: "#fff",
                boxShadow: "0 5px 15px rgba(0,0,0,0.08)",
                display: "flex",
                flexDirection: "column",
                gap: 1,
            }}
        >
            {icon && (
                <Typography>
                    {icon}
                </Typography>
            )}
            <Typography
                sx={{
                    color: "#7A6A86",
                    fontSize: "0.85rem",
                }}
            >
                {title}
            </Typography>
            <Typography
                variant="h5"
                sx={{
                    color: "#4B3553",
                    fontWeight: 700,
                }}
            >
                {value}
            </Typography>
        </Paper>
    );
};

export default DashboardCard;
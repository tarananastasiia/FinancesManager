import React from "react";
import {
    PieChart,
    Pie,
    Cell,
    Legend,
    Tooltip,
    ResponsiveContainer
} from "recharts";
import { Box, Paper, Typography } from "@mui/material";

interface Props {
    monthlySpending: {
        month: string;
        amount: number;
    }[];
    categorySpending: {
        category: string;
        amount: number;
    }[];
}

const COLORS = [
    "#8884d8",
    "#82ca9d",
    "#ffc658",
    "#ff8042",
    "#0088FE",
    "#FFBB28"
];

const SpendingCharts: React.FC<Props> = ({categorySpending}) => {
    const renderLabel = ({
        name,
        percent
    }: any) => {
        return `${name} ${(percent * 100).toFixed(0)}%`;
    };

    return (
        <Box
            sx={{
                display: "grid",
                gridTemplateColumns: {
                    xs: "1fr",
                    md: "1fr 1fr"
                },
                gap: 3,
                mt: 3
            }}
        >
            <Paper sx={{ p: 3, borderRadius: 3 }}>
                <Typography variant="h6">
                    Spending Categories
                </Typography>
                <ResponsiveContainer width="100%" height={250}>
                    <PieChart>
                        <Pie
                            data={categorySpending}
                            dataKey="amount"
                            nameKey="category"
                            outerRadius={90}
                            label={renderLabel}
                        >
                            {
                                categorySpending.map((entry, index) => (
                                    <Cell
                                        key={entry.category}
                                        fill={COLORS[index % COLORS.length]}
                                    />
                                ))
                            }
                        </Pie>
                        <Tooltip
                            formatter={(value) =>
                                `$${Number(value ?? 0).toFixed(2)}`
                            }
                        />
                        <Legend />
                    </PieChart>
                </ResponsiveContainer>
            </Paper>
        </Box>
    );
};

export default SpendingCharts;
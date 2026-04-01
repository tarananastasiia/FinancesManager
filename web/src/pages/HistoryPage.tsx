import { useEffect, useState } from "react";
import { getHistory } from "../api/paymentApi";

export default function HistoryPage() {

  const [data, setData] = useState<any[]>([]);

  useEffect(() => {
    getHistory().then(setData);
  }, []);

  return (
    <div>

      <h2>Transactions</h2>

      {data.map(t => (
        <div key={t.id}>
          💳 {t.card} —
          ${t.amount} —
          {t.status} —
          {new Date(t.date).toLocaleString()}
        </div>
      ))}

    </div>
  );
}
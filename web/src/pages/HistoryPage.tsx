import { useEffect, useState } from "react";
import { getHistoryApi } from "../api/paymentApi";

export default function HistoryPage() {

  const [data, setData] = useState<any[]>([]);

  useEffect(() => {
    getHistoryApi().then(setData);
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
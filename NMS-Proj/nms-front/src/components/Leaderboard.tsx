import { useState, useEffect } from 'react';
import { api } from '../services/api';
import { Explorador } from '../types';

export default function Leaderboard() {
  const [exploradores, setExploradores] = useState<Explorador[]>([]);

  useEffect(() => {
    loadLeaderboard();
  }, []);

  const loadLeaderboard = async () => {
    const data = await api.getLeaderboard();
    setExploradores(data);
  };

  return (
    <div className="card">
      <h2>🏆 Leaderboard</h2>
      <table>
        <thead>
          <tr>
            <th>Posição</th>
            <th>Nome</th>
            <th>Pontos</th>
          </tr>
        </thead>
        <tbody>
          {exploradores.map((exp, index) => (
            <tr key={exp.id}>
              <td>{index + 1}º</td>
              <td>{exp.nome}</td>
              <td>{exp.pontuacao}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

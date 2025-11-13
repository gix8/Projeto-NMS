import { useState, useEffect } from 'react';
import { api } from '../services/api';
import { Explorador } from '../types';

export default function SistemaForm({ onSuccess }: { onSuccess: () => void }) {
  const [nome, setNome] = useState('');
  const [qntdPlanetas, setQntdPlanetas] = useState(1);
  const [exploradorId, setExploradorId] = useState('');
  const [exploradores, setExploradores] = useState<Explorador[]>([]);

  useEffect(() => {
    loadExploradores();
  }, []);

  const loadExploradores = async () => {
    const data = await api.getExploradores();
    setExploradores(data);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await api.createSistema({ 
      nome, 
      qntdPlanetas, 
      exploradorId: parseInt(exploradorId) 
    });
    setNome('');
    setQntdPlanetas(1);
    onSuccess();
  };

  return (
    <div className="card">
      <h2>🌌 Criar Sistema Estelar</h2>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          value={nome}
          onChange={(e) => setNome(e.target.value)}
          placeholder="Nome do sistema"
          required
        />
        <input
          type="number"
          value={qntdPlanetas}
          onChange={(e) => setQntdPlanetas(parseInt(e.target.value))}
          min="1"
          placeholder="Quantidade de planetas"
        />
        <select 
          value={exploradorId} 
          onChange={(e) => setExploradorId(e.target.value)} 
          required
        >
          <option value="">Selecione o explorador</option>
          {exploradores.map(exp => (
            <option key={exp.id} value={exp.id}>{exp.nome}</option>
          ))}
        </select>
        <button type="submit">Criar Sistema</button>
      </form>
    </div>
  );
}

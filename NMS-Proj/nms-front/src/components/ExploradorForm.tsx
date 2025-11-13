import { useState } from 'react';
import { api } from '../services/api';

export default function ExploradorForm({ onSuccess }: { onSuccess: () => void }) {
  const [nome, setNome] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!nome) return;
    
    await api.createExplorador(nome);
    setNome('');
    onSuccess();
  };

  return (
    <div className="card">
      <h2>👤 Criar Explorador</h2>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          value={nome}
          onChange={(e) => setNome(e.target.value)}
          placeholder="Nome do explorador"
          required
        />
        <button type="submit">Criar</button>
      </form>
    </div>
  );
}

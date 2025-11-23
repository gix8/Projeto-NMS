import { useState, useEffect } from 'react';
import { api } from '../services/api';
import { Explorador } from '../types';

export default function ManageExploradores({ onUpdate }: { onUpdate: () => void }) {
  const [exploradores, setExploradores] = useState<Explorador[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    loadExploradores();
  }, []);

  const loadExploradores = async () => {
    setLoading(true);
    try {
      const data = await api.getExploradores();
      setExploradores(data);
    } catch (error) {
      console.error('Erro ao carregar exploradores:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number, nome: string) => {
    const confirmDelete = window.confirm(
      `⚠️ ATENÇÃO!\n\nVocê tem certeza que deseja deletar o explorador "${nome}"?\n\n` +
      `Isso também irá deletar:\n` +
      `• Todos os sistemas descobertos por este explorador\n` +
      `• Todos os planetas explorados por este explorador\n\n` +
      `Esta ação NÃO pode ser desfeita!`
    );

    if (!confirmDelete) return;

    setLoading(true);
    try {
      const success = await api.deleteExplorador(id);
      if (success) {
        alert(`✅ Explorador "${nome}" deletado com sucesso!`);
        loadExploradores();
        onUpdate();
      } else {
        alert('❌ Erro ao deletar explorador.');
      }
    } catch (error) {
      console.error('Erro ao deletar:', error);
      alert('❌ Erro ao deletar explorador.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="card manage-section">
      <h2>🗑️ Gerenciar Exploradores</h2>
      
      {loading && <p className="loading">Carregando...</p>}
      
      {!loading && exploradores.length === 0 && (
        <p className="no-data">Nenhum explorador cadastrado</p>
      )}

      {!loading && exploradores.length > 0 && (
        <div className="manage-list">
          {exploradores.map((exp) => (
            <div key={exp.id} className="manage-item">
              <div className="item-info">
                <span className="item-name">{exp.nome}</span>
                <span className="item-details">
                  ID: {exp.id} | {exp.pontuacao} pontos
                </span>
              </div>
              <button 
                className="btn-delete"
                onClick={() => handleDelete(exp.id, exp.nome)}
                disabled={loading}
              >
                🗑️ Deletar
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
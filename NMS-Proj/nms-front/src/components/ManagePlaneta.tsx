import { useState, useEffect } from 'react';
import { api } from '../services/api';
import { Planeta } from '../types';

export default function ManagePlanetas({ onUpdate }: { onUpdate: () => void }) {
  const [planetas, setPlanetas] = useState<Planeta[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    loadPlanetas();
  }, []);

  const loadPlanetas = async () => {
    setLoading(true);
    try {
      // A API retorna planetas com informações completas
      const data = await api.getPlanetas();
      setPlanetas(data);
    } catch (error) {
      console.error('Erro ao carregar planetas:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (planeta: Planeta) => {
    // Preparar valores com fallback de casing
    const sistemaIdPreview = (planeta as any).SistemaEstelarId ?? (planeta as any).sistemaEstelarId ?? (planeta as any).sistemaId;
    const nomePreview = (planeta as any).nome ?? (planeta as any).Nome ?? (planeta as any).Nome;

    const confirmDelete = window.confirm(
      `⚠️ ATENÇÃO!\n\nVocê tem certeza que deseja deletar o planeta "${nomePreview}"?\n\n` +
      `Sistema: ID ${sistemaIdPreview}\n` +
      `Esta ação NÃO pode ser desfeita!`
    );

    if (!confirmDelete) return;

    setLoading(true);
    try {
      // Suportar diferentes formatos de casing vindos do backend (Nome vs nome, SistemaEstelarId vs sistemaEstelarId)
      const sistemaId = (planeta as any).SistemaEstelarId ?? (planeta as any).sistemaEstelarId ?? (planeta as any).sistemaId;
      const nome = (planeta as any).nome ?? (planeta as any).Nome ?? (planeta as any).Nome;

      const success = await api.deletePlaneta(sistemaId, nome);
      if (success) {
        alert(`✅ Planeta "${nomePreview}" deletado com sucesso!`);
        loadPlanetas();
        onUpdate();
      } else {
        alert('❌ Erro ao deletar planeta.');
      }
    } catch (error) {
      console.error('Erro ao deletar:', error);
      alert('❌ Erro ao deletar planeta.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="card manage-section">
      <h2>🗑️ Gerenciar Planetas</h2>
      
      {loading && <p className="loading">Carregando...</p>}
      
      {!loading && planetas.length === 0 && (
        <p className="no-data">Nenhum planeta cadastrado</p>
      )}

      {!loading && planetas.length > 0 && (
        <div className="manage-list">
          {planetas.map((planeta, index) => (
            <div key={index} className="manage-item">
              <div className="item-info">
                <span className="item-name">{(planeta as any).nome ?? (planeta as any).Nome}</span>
                <span className="item-details">
                  Sistema ID: {(planeta as any).SistemaEstelarId ?? (planeta as any).sistemaEstelarId} | 
                  Explorador ID: {(planeta as any).exploradorId ?? (planeta as any).ExploradorId} |
                  Recursos: {planeta.recursos || (planeta as any).Recursos || 'Nenhum'}
                </span>
              </div>
              <button 
                className="btn-delete"
                onClick={() => handleDelete(planeta)}
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
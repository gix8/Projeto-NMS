import { useState, useEffect } from 'react';
import { api } from '../services/api';
import { SistemaEstelar } from '../types';

export default function ManageSistemas({ onUpdate }: { onUpdate: () => void }) {
  const [sistemas, setSistemas] = useState<SistemaEstelar[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    loadSistemas();
  }, []);

  const loadSistemas = async () => {
    setLoading(true);
    try {
      const data = await api.getSistemas();
      setSistemas(data);
    } catch (error) {
      console.error('Erro ao carregar sistemas:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number, nome: string) => {
    const confirmDelete = window.confirm(
      `⚠️ ATENÇÃO!\n\nVocê tem certeza que deseja deletar o sistema "${nome}"?\n\n` +
      `Isso também irá deletar:\n` +
      `• Todos os planetas deste sistema\n\n` +
      `Esta ação NÃO pode ser desfeita!`
    );

    if (!confirmDelete) return;

    setLoading(true);
    try {
      const success = await api.deleteSistema(id);
      if (success) {
        alert(`✅ Sistema "${nome}" deletado com sucesso!`);
        loadSistemas();
        onUpdate();
      } else {
        alert('❌ Erro ao deletar sistema.');
      }
    } catch (error) {
      console.error('Erro ao deletar:', error);
      alert('❌ Erro ao deletar sistema.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="card manage-section">
      <h2>🗑️ Gerenciar Sistemas Estelares</h2>
      
      {loading && <p className="loading">Carregando...</p>}
      
      {!loading && sistemas.length === 0 && (
        <p className="no-data">Nenhum sistema cadastrado</p>
      )}

      {!loading && sistemas.length > 0 && (
        <div className="manage-list">
          {sistemas.map((sistema) => (
            <div key={sistema.id} className="manage-item">
              <div className="item-info">
                <span className="item-name">{sistema.nome}</span>
                <span className="item-details">
                  ID: {sistema.id} | {sistema.qntdPlanetas} planetas | 
                  Descoberto por: {sistema.exploradorNome || 'N/A'}
                </span>
              </div>
              <button 
                className="btn-delete"
                onClick={() => handleDelete(sistema.id, sistema.nome)}
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
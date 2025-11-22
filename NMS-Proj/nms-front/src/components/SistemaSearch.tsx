import { useState } from 'react';
import { api } from '../services/api';
import { SistemaDetalhado } from '../types';

export default function SistemaSearch() {
  const [sistemaId, setSistemaId] = useState('');
  const [sistema, setSistema] = useState<SistemaDetalhado | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!sistemaId) return;

    setLoading(true);
    setError('');
    setSistema(null);

    try {
      const data = await api.getSistemaById(parseInt(sistemaId));
      setSistema(data);
    } catch (err) {
      setError('Sistema não encontrado');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="card">
      <h2>🔍 Pesquisar Sistema Estelar</h2>
      
      <form onSubmit={handleSearch}>
        <input
          type="number"
          value={sistemaId}
          onChange={(e) => setSistemaId(e.target.value)}
          placeholder="ID do Sistema"
          min="1"
          required
        />
        <button type="submit" disabled={loading}>
          {loading ? 'Buscando...' : 'Buscar'}
        </button>
      </form>

      {error && <p className="error">{error}</p>}

      {sistema && (
        <div className="search-result">
          <div className="sistema-info">
            <h3>🌌 {sistema.nome}</h3>
            <p><strong>ID:</strong> {sistema.id}</p>
            <p><strong>Planetas Declarados:</strong> {sistema.qntdPlanetas}</p>
            <p><strong>Descoberto por:</strong> {sistema.exploradorNome || 'Desconhecido'} (ID: {sistema.exploradorId})</p>
          </div>

          <div className="planetas-section">
            <h4>🪐 Planetas no Sistema ({sistema.planetas?.length || 0})</h4>
            {sistema.planetas && sistema.planetas.length > 0 ? (
              <div className="planetas-grid">
                {sistema.planetas.map((planeta, index) => (
                  <div key={index} className="planeta-card">
                    <h5>{planeta.nome}</h5>
                    <div className="planeta-details">
                      <p><strong>☀️ Clima:</strong> {planeta.clima} 
                        <span className={`quality ${planeta.climaQualidade}`}>
                          {planeta.climaQualidade === 'bom' ? '✓' : '✗'}
                        </span>
                      </p>
                      <p><strong>🦎 Fauna:</strong> {planeta.fauna}
                        <span className={`quality ${planeta.faunaQualidade}`}>
                          {planeta.faunaQualidade === 'bom' ? '✓' : '✗'}
                        </span>
                      </p>
                      <p><strong>🌿 Flora:</strong> {planeta.flora}
                        <span className={`quality ${planeta.floraQualidade}`}>
                          {planeta.floraQualidade === 'bom' ? '✓' : '✗'}
                        </span>
                      </p>
                      <p><strong>🤖 Sentinelas:</strong> {planeta.sentinelas}
                        <span className={`quality ${planeta.sentinelasQualidade}`}>
                          {planeta.sentinelasQualidade === 'bom' ? '✓' : '✗'}
                        </span>
                      </p>
                      <p><strong>💎 Recursos:</strong> {planeta.recursos || 'Nenhum'}</p>
                    </div>
                  </div>
                ))}
              </div>
            ) : (
              <p className="no-data">Nenhum planeta cadastrado neste sistema</p>
            )}
          </div>
        </div>
      )}
    </div>
  );
}
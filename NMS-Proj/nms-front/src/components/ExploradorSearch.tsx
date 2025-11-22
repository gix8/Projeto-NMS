import { useState } from 'react';
import { api } from '../services/api';
import { ExploradorDetalhado } from '../types';

export default function ExploradorSearch() {
  const [searchTerm, setSearchTerm] = useState('');
  const [searchType, setSearchType] = useState<'id' | 'nome'>('nome');
  const [explorador, setExplorador] = useState<ExploradorDetalhado | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!searchTerm) return;

    setLoading(true);
    setError('');
    setExplorador(null);

    try {
      let data;
      if (searchType === 'id') {
        data = await api.getExploradorById(parseInt(searchTerm));
      } else {
        data = await api.searchExploradorByName(searchTerm);
      }
      setExplorador(data);
    } catch (err) {
      setError('Explorador não encontrado');
    } finally {
      setLoading(false);
    }
  };

  const getTotalPontos = () => {
    if (!explorador) return 0;
    return explorador.explorador.pontuacao;
  };

  return (
    <div className="card explorador-search">
      <h2>👤 Pesquisar Explorador</h2>
      
      <form onSubmit={handleSearch}>
        <div className="search-options">
          <label>
            <input
              type="radio"
              value="nome"
              checked={searchType === 'nome'}
              onChange={(e) => setSearchType('nome')}
            />
            Buscar por Nome
          </label>
          <label>
            <input
              type="radio"
              value="id"
              checked={searchType === 'id'}
              onChange={(e) => setSearchType('id')}
            />
            Buscar por ID
          </label>
        </div>

        <input
          type={searchType === 'id' ? 'number' : 'text'}
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          placeholder={searchType === 'id' ? 'ID do Explorador' : 'Nome do Explorador'}
          required
        />
        <button type="submit" disabled={loading}>
          {loading ? 'Buscando...' : 'Buscar'}
        </button>
      </form>

      {error && <p className="error">{error}</p>}

      {explorador && (
        <div className="search-result">
          <div className="explorador-info">
            <h3>👨‍🚀 {explorador.explorador.nome}</h3>
            <div className="explorador-stats">
              <div className="stat">
                <span className="stat-label">ID:</span>
                <span className="stat-value">{explorador.explorador.id}</span>
              </div>
              <div className="stat">
                <span className="stat-label">Pontuação Total:</span>
                <span className="stat-value highlight">{getTotalPontos()} pts</span>
              </div>
              <div className="stat">
                <span className="stat-label">Sistemas Descobertos:</span>
                <span className="stat-value">{explorador.sistemasEstelares?.length || 0}</span>
              </div>
              <div className="stat">
                <span className="stat-label">Planetas Explorados:</span>
                <span className="stat-value">{explorador.planetasExplorados?.length || 0}</span>
              </div>
            </div>
          </div>

          {/* Sistemas Descobertos */}
          <div className="descobertas-section">
            <h4>🌌 Sistemas Estelares Descobertos</h4>
            {explorador.sistemasEstelares && explorador.sistemasEstelares.length > 0 ? (
              <div className="sistemas-list">
                {explorador.sistemasEstelares.map((sistema) => (
                  <div key={sistema.id} className="sistema-item">
                    <div className="sistema-header">
                      <span className="sistema-name">{sistema.nome}</span>
                      <span className="sistema-badge">ID: {sistema.id}</span>
                    </div>
                    <div className="sistema-details">
                      <span>📊 {sistema.qntdPlanetas} planetas declarados</span>
                    </div>
                  </div>
                ))}
              </div>
            ) : (
              <p className="no-data">Nenhum sistema descoberto ainda</p>
            )}
          </div>

          {/* Planetas Explorados */}
          <div className="descobertas-section">
            <h4>🪐 Planetas Explorados</h4>
            {explorador.planetasExplorados && explorador.planetasExplorados.length > 0 ? (
              <div className="planetas-list">
                {explorador.planetasExplorados.map((planeta, index) => (
                  <div key={index} className="planeta-item">
                    <div className="planeta-header">
                      <span className="planeta-name">{planeta.nome}</span>
                      <span className="planeta-sistema">Sistema ID: {planeta.sistemaEstelarId}</span>
                    </div>
                    <div className="planeta-info-grid">
                      <div className="info-item">
                        <span className="icon">☀️</span>
                        <span>{planeta.clima || 'N/A'}</span>
                      </div>
                      <div className="info-item">
                        <span className="icon">🦎</span>
                        <span>{planeta.fauna || 'N/A'}</span>
                      </div>
                      <div className="info-item">
                        <span className="icon">🌿</span>
                        <span>{planeta.flora || 'N/A'}</span>
                      </div>
                      <div className="info-item">
                        <span className="icon">🤖</span>
                        <span>{planeta.sentinelas || 'N/A'}</span>
                      </div>
                    </div>
                    {planeta.recursos && (
                      <div className="recursos-tag">
                        💎 {planeta.recursos}
                      </div>
                    )}
                  </div>
                ))}
              </div>
            ) : (
              <p className="no-data">Nenhum planeta explorado ainda</p>
            )}
          </div>
        </div>
      )}
    </div>
  );
}
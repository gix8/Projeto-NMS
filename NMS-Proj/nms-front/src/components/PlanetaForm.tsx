import { useState, useEffect } from 'react';
import { api } from '../services/api';
import { Explorador, SistemaEstelar } from '../types';

export default function PlanetaForm({ onSuccess }: { onSuccess: () => void }) {
  const [formData, setFormData] = useState({
    nome: '',
    clima: '',
    climaQualidade: 'bom',
    fauna: '',
    faunaQualidade: 'bom',
    flora: '',
    floraQualidade: 'bom',
    sentinelas: '',
    sentinelasQualidade: 'bom',
    recursos: '',
    sistemaEstelarId: '',
    exploradorId: ''
  });
  const [exploradores, setExploradores] = useState<Explorador[]>([]);
  const [sistemas, setSistemas] = useState<SistemaEstelar[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const [exploradoresData, sistemasData] = await Promise.all([
        api.getExploradores(),
        api.getSistemas()
      ]);
      setExploradores(exploradoresData);
      setSistemas(sistemasData);
    } catch (error) {
      console.error('Erro ao carregar dados:', error);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    
    try {
      await api.createPlaneta({
        ...formData,
        sistemaEstelarId: parseInt(formData.sistemaEstelarId),
        exploradorId: parseInt(formData.exploradorId)
      });
      
      setFormData({
        nome: '', clima: '', climaQualidade: 'bom',
        fauna: '', faunaQualidade: 'bom', flora: '', floraQualidade: 'bom',
        sentinelas: '', sentinelasQualidade: 'bom', recursos: '',
        sistemaEstelarId: '', exploradorId: ''
      });
      
      onSuccess();
    } catch (error) {
      console.error('Erro ao criar planeta:', error);
      alert('Erro ao criar planeta. Verifique os dados e tente novamente.');
    } finally {
      setLoading(false);
    }
  };

  // Busca o nome do explorador que descobriu o sistema
  const getSistemaOwner = (sistemaId: number) => {
    const sistema = sistemas.find(s => s.id === sistemaId);
    if (!sistema) return null;
    
    // usar o nome do explorador já presente no objeto sistema 
    return sistema.exploradorNome || 'Desconhecido';
  };

  return (
    <div className="card planeta-form">
      <h2>🪐 Criar Planeta</h2>
      <form onSubmit={handleSubmit}>
        
        <div className="form-group">
          <label htmlFor="nome">Nome do Planeta *</label>
          <input 
            id="nome"
            type="text" 
            value={formData.nome} 
            onChange={(e) => setFormData({...formData, nome: e.target.value})}
            placeholder="Ex: Terra, Marte, Tatooine"
            required 
          />
        </div>

        <div className="form-group">
          <label htmlFor="sistema">Sistema Estelar *</label>
          <select 
            id="sistema"
            value={formData.sistemaEstelarId}
            onChange={(e) => setFormData({...formData, sistemaEstelarId: e.target.value})}
            required
          >
            <option value="">Selecione o sistema</option>
            {sistemas.map(sistema => (
              <option key={sistema.id} value={sistema.id}>
                {sistema.nome} (ID: {sistema.id}) - Descoberto por: {sistema.exploradorNome || 'N/A'}
              </option>
            ))}
          </select>
          {sistemas.length === 0 && (
            <small className="helper-text warning">
              ⚠️ Nenhum sistema cadastrado. Crie um sistema primeiro.
            </small>
          )}
          {formData.sistemaEstelarId && (
            <small className="helper-text info">
              ℹ️ Sistema descoberto por: {getSistemaOwner(parseInt(formData.sistemaEstelarId))}
            </small>
          )}
        </div>

        <div className="form-group">
          <label htmlFor="explorador">Explorador (Você) *</label>
          <select 
            id="explorador"
            value={formData.exploradorId}
            onChange={(e) => setFormData({...formData, exploradorId: e.target.value})} 
            required
          >
            <option value="">Quem está explorando?</option>
            {exploradores.map(exp => (
              <option key={exp.id} value={exp.id}>
                {exp.nome} (ID: {exp.id}) - {exp.pontuacao} pts
              </option>
            ))}
          </select>
        </div>

        <div className="form-divider">
          <span>Características do Planeta</span>
        </div>

        <div className="form-row">
          <div className="form-group">
            <label htmlFor="clima">Clima</label>
            <input 
              id="clima"
              type="text" 
              value={formData.clima}
              onChange={(e) => setFormData({...formData, clima: e.target.value})}
              placeholder="Ex: Temperado, Árido"
            />
          </div>
          <div className="form-group">
            <label htmlFor="climaQualidade">Qualidade</label>
            <select 
              id="climaQualidade"
              value={formData.climaQualidade}
              onChange={(e) => setFormData({...formData, climaQualidade: e.target.value})}
            >
              <option value="bom">✓ Bom</option>
              <option value="ruim">✗ Ruim</option>
            </select>
          </div>
        </div>

        <div className="form-row">
          <div className="form-group">
            <label htmlFor="fauna">Fauna</label>
            <input 
              id="fauna"
              type="text" 
              value={formData.fauna}
              onChange={(e) => setFormData({...formData, fauna: e.target.value})}
              placeholder="Ex: Rica, Escassa"
            />
          </div>
          <div className="form-group">
            <label htmlFor="faunaQualidade">Qualidade</label>
            <select 
              id="faunaQualidade"
              value={formData.faunaQualidade}
              onChange={(e) => setFormData({...formData, faunaQualidade: e.target.value})}
            >
              <option value="bom">✓ Bom</option>
              <option value="ruim">✗ Ruim</option>
            </select>
          </div>
        </div>

        <div className="form-row">
          <div className="form-group">
            <label htmlFor="flora">Flora</label>
            <input 
              id="flora"
              type="text" 
              value={formData.flora}
              onChange={(e) => setFormData({...formData, flora: e.target.value})}
              placeholder="Ex: Abundante, Rara"
            />
          </div>
          <div className="form-group">
            <label htmlFor="floraQualidade">Qualidade</label>
            <select 
              id="floraQualidade"
              value={formData.floraQualidade}
              onChange={(e) => setFormData({...formData, floraQualidade: e.target.value})}
            >
              <option value="bom">✓ Bom</option>
              <option value="ruim">✗ Ruim</option>
            </select>
          </div>
        </div>

        <div className="form-row">
          <div className="form-group">
            <label htmlFor="sentinelas">Sentinelas</label>
            <input 
              id="sentinelas"
              type="text" 
              value={formData.sentinelas}
              onChange={(e) => setFormData({...formData, sentinelas: e.target.value})}
              placeholder="Ex: Passivas, Agressivas"
            />
          </div>
          <div className="form-group">
            <label htmlFor="sentinelasQualidade">Qualidade</label>
            <select 
              id="sentinelasQualidade"
              value={formData.sentinelasQualidade}
              onChange={(e) => setFormData({...formData, sentinelasQualidade: e.target.value})}
            >
              <option value="bom">✓ Bom</option>
              <option value="ruim">✗ Ruim</option>
            </select>
          </div>
        </div>

        <div className="form-group">
          <label htmlFor="recursos">Recursos</label>
          <input 
            id="recursos"
            type="text" 
            value={formData.recursos}
            onChange={(e) => setFormData({...formData, recursos: e.target.value})}
            placeholder="Ex: Ouro, Platina, Diamante (separados por vírgula)"
          />
        </div>

        <button type="submit" disabled={loading}>
          {loading ? 'Criando...' : 'Criar Planeta'}
        </button>
      </form>
    </div>
  );
}
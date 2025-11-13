import { useState, useEffect } from 'react';
import { api } from '../services/api';
import { Explorador } from '../types';

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

  useEffect(() => {
    loadExploradores();
  }, []);

  const loadExploradores = async () => {
    const data = await api.getExploradores();
    setExploradores(data);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
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
  };

  return (
    <div className="card">
      <h2>🪐 Criar Planeta</h2>
      <form onSubmit={handleSubmit}>
        <input 
          type="text" 
          value={formData.nome} 
          onChange={(e) => setFormData({...formData, nome: e.target.value})}
          placeholder="Nome do planeta" 
          required 
        />
        
        <input 
          type="number" 
          value={formData.sistemaEstelarId}
          onChange={(e) => setFormData({...formData, sistemaEstelarId: e.target.value})}
          placeholder="ID do Sistema" 
          required 
        />
        
        <select 
          value={formData.exploradorId}
          onChange={(e) => setFormData({...formData, exploradorId: e.target.value})} 
          required
        >
          <option value="">Selecione explorador</option>
          {exploradores.map(exp => (
            <option key={exp.id} value={exp.id}>{exp.nome}</option>
          ))}
        </select>

        <input 
          type="text" 
          value={formData.clima}
          onChange={(e) => setFormData({...formData, clima: e.target.value})}
          placeholder="Clima" 
        />
        <select 
          value={formData.climaQualidade}
          onChange={(e) => setFormData({...formData, climaQualidade: e.target.value})}
        >
          <option value="bom">Clima Bom</option>
          <option value="ruim">Clima Ruim</option>
        </select>

        <input 
          type="text" 
          value={formData.fauna}
          onChange={(e) => setFormData({...formData, fauna: e.target.value})}
          placeholder="Fauna" 
        />
        <select 
          value={formData.faunaQualidade}
          onChange={(e) => setFormData({...formData, faunaQualidade: e.target.value})}
        >
          <option value="bom">Fauna Boa</option>
          <option value="ruim">Fauna Ruim</option>
        </select>

        <input 
          type="text" 
          value={formData.flora}
          onChange={(e) => setFormData({...formData, flora: e.target.value})}
          placeholder="Flora" 
        />
        <select 
          value={formData.floraQualidade}
          onChange={(e) => setFormData({...formData, floraQualidade: e.target.value})}
        >
          <option value="bom">Flora Boa</option>
          <option value="ruim">Flora Ruim</option>
        </select>

        <input 
          type="text" 
          value={formData.sentinelas}
          onChange={(e) => setFormData({...formData, sentinelas: e.target.value})}
          placeholder="Sentinelas" 
        />
        <select 
          value={formData.sentinelasQualidade}
          onChange={(e) => setFormData({...formData, sentinelasQualidade: e.target.value})}
        >
          <option value="bom">Sentinelas Boas</option>
          <option value="ruim">Sentinelas Ruins</option>
        </select>

        <input 
          type="text" 
          value={formData.recursos}
          onChange={(e) => setFormData({...formData, recursos: e.target.value})}
          placeholder="Recursos (separados por vírgula)" 
        />

        <button type="submit">Criar Planeta</button>
      </form>
    </div>
  );
}



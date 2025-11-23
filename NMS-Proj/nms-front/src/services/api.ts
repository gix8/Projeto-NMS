const API_URL = 'http://localhost:5259';

export const api = {
  // Exploradores
  async getExploradores() {
    const response = await fetch(`${API_URL}/exploradores`);
    return response.json();
  },

  async createExplorador(nome: string) {
    const response = await fetch(`${API_URL}/exploradores`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ nome })
    });
    return response.json();
  },

  async deleteExplorador(id: number) {
    const response = await fetch(`${API_URL}/exploradores/${id}`, {
      method: 'DELETE'
    });
    return response.ok;
  },

  async getLeaderboard() {
    const response = await fetch(`${API_URL}/exploradores/leaderboard`);
    return response.json();
  },

  // Sistemas
  async createSistema(data: any) {
    const response = await fetch(`${API_URL}/sistemas`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    });
    return response.json();
  },

  async deleteSistema(id: number) {
    const response = await fetch(`${API_URL}/sistemas/${id}`, {
      method: 'DELETE'
    });
    return response.ok;
  },

  // Planetas
  async createPlaneta(data: any) {
    const response = await fetch(`${API_URL}/planetas`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    });
    return response.json();
  },

  async getPlanetas() {
    const response = await fetch(`${API_URL}/planetas`);
    return response.json();
  },

  async deletePlaneta(sistemaId: number, nome: string) {
    const response = await fetch(`${API_URL}/planetas/${sistemaId}/${encodeURIComponent(nome)}`, {
      method: 'DELETE'
    });
    return response.ok;
  },

  async getSistemaById(id: number) {
  const response = await fetch(`${API_URL}/sistemas/${id}`);
  return response.json();
  },

  async getSistemas() {
  const response = await fetch(`${API_URL}/sistemas`);
  return response.json();
  },
  
  async getExploradorById(id: number) {
  const response = await fetch(`${API_URL}/exploradores/${id}`);
  return response.json();
  },

  async searchExploradorByName(nome: string) {
  const response = await fetch(`${API_URL}/exploradores/pesquisa/${encodeURIComponent(nome)}`);
  return response.json();
  }
};
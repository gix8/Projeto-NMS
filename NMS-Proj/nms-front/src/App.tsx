import './styles/main.css';
import { useState } from 'react';
import './App.css';
import Leaderboard from './components/Leaderboard';
import ExploradorForm from './components/ExploradorForm';
import SistemaForm from './components/SistemaForm';
import PlanetaForm from './components/PlanetaForm';
import SistemaSearch from './components/SistemaSearch';
import ExploradorSearch from './components/ExploradorSearch';
import ManageExploradores from './components/ManageExplorador';  
import ManageSistemas from './components/ManageSistema';          
import ManagePlanetas from './components/ManagePlaneta'; 

function App() {
  const [refresh, setRefresh] = useState(0);

  const handleSuccess = () => {
    setRefresh((prev: number) => prev + 1);
  };

  return (
    <div className="App">
      <h1>🚀 NMS Explorer Manager</h1>
      
      <div className="grid">
        <Leaderboard key={refresh} />
        <ExploradorForm onSuccess={handleSuccess} />
      </div>

      <div className="grid">
        <SistemaSearch />
        <ExploradorSearch />
      </div>

      <div className="grid">
        <SistemaForm onSuccess={handleSuccess} />
        <PlanetaForm onSuccess={handleSuccess} />
      </div>

      <div className="section-title">
        <h2>🗑️ Gerenciamento de Dados</h2>
      </div>
      <div className="grid">
        <ManageExploradores onUpdate={handleSuccess} />
        <ManageSistemas onUpdate={handleSuccess} />
      </div>
      <ManagePlanetas onUpdate={handleSuccess} />
    </div>
  );
}

export default App;

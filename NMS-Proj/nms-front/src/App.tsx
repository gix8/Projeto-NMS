import './styles/main.css';
import { useState } from 'react';
import './App.css';
import Leaderboard from './components/Leaderboard';
import ExploradorForm from './components/ExploradorForm';
import SistemaForm from './components/SistemaForm';
import PlanetaForm from './components/PlanetaForm';

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
        <SistemaForm onSuccess={handleSuccess} />
        <PlanetaForm onSuccess={handleSuccess} />
      </div>
    </div>
  );
}

export default App;

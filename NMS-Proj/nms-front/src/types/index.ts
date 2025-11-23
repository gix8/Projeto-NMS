
export interface Explorador {
    id: number;
    nome: string;
    pontuacao: number;
}

export interface SistemaEstelar {   
    id: number;
    nome: string;
    qntdPlanetas: number;
    pontuacao: number;
    exploradorId: number;
    exploradorNome: string;
}

export interface Planeta {
    nome: string;
    clima: string;
    climaQualidade: string;
    fauna: string;
    faunaQualidade: string;
    flora: string;
    floraQualidade: string;
    sentinelas: string;
    sentinelasQualidade: string;
    recursos: string;
    SistemaEstelarId: number;
    exploradorId: number;
}

export interface SistemaDetalhado {
  id: number;
  nome: string;
  qntdPlanetas: number;
  exploradorId: number;
  exploradorNome: string | null;
  planetas: Planeta[];
}

export interface PlanetaExplorador {
  nome: string;
  sistemaEstelarId: number;
  clima: string;
  fauna: string;
  flora: string;
  sentinelas: string;
  recursos: string;
}

export interface SistemaExplorador {
  id: number;
  nome: string;
  qntdPlanetas: number;
}

export interface ExploradorDetalhado {
  explorador: {
    id: number;
    nome: string;
    pontuacao: number;
  };
  sistemasEstelares: SistemaExplorador[];
  planetasExplorados: PlanetaExplorador[];
}
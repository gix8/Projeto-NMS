
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
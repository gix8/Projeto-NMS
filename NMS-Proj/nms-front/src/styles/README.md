# Documentação - Sistema de Estilos Espaciais (NMS)

## 📚 Visão Geral

Sistema completo de CSS com temática espacial para o projeto NMS, sem uso de frameworks como Tailwind. Todos os estilos utilizam variáveis CSS customizadas e seguem um padrão consistente.

---

## 🎨 Estrutura de Arquivos

```
src/styles/
├── reset.css          # Reset de estilos padrão
├── variables.css      # Variáveis CSS (cores, espaçamentos, fontes)
├── typography.css     # Estilos de tipografia
├── layout.css         # Grid, Flexbox e Containers
├── components.css     # Botões, Cards, Inputs
├── animations.css     # Animações e efeitos especiais
└── main.css          # Arquivo principal (importa todos)
```

---

## 🚀 Como Usar

### 1. Importar no seu React Component

```tsx
// No seu arquivo principal (App.tsx ou index.tsx)
import './styles/main.css';
```

### 2. Usar Classes CSS

```tsx
export function MyComponent() {
  return (
    <div className="container-lg">
      <h1>Bem-vindo ao NMS</h1>
      <button className="btn btn-primary">Explorar</button>
    </div>
  );
}
```

---

## 🎨 Paleta de Cores

### Cores Primárias

| Variável | Valor | Uso |
|----------|-------|-----|
| `--color-primary` | #1a1f3a | Fundo padrão, elementos primários |
| `--color-secondary` | #7c3aed | Botões, bordas, destaques |
| `--color-accent-cyan` | #06b6d4 | Links, hover, glow effects |
| `--color-accent-gold` | #f59e0b | Labels, destaque especial |
| `--color-bg-dark` | #0a0e1a | Background cosmos |

### Acessar as Cores

```css
.meu-elemento {
  color: var(--color-accent-cyan);
  background: var(--color-primary);
  box-shadow: var(--glow-cyan);
}
```

---

## 📐 Tipografia

### Headings

```tsx
<h1>Título Principal</h1>    {/* 48px, gradient neon */}
<h2>Subtítulo</h2>          {/* 36px, cyan glow */}
<h3>Seção</h3>              {/* 30px, purple */}
<h4>Subsection</h4>         {/* 24px */}
<h5>Pequeno Título</h5>     {/* 20px */}
<h6>Label</h6>              {/* 16px, uppercase gold */}
```

### Estilos de Texto

```tsx
<strong>Texto em Destaque</strong>  {/* Dourado */}
<em>Itálico</em>                    {/* Cyan itálico */}
<code>const x = 10;</code>          {/* Fonte monospace, purple bg */}
<mark>Marcado</mark>                {/* Fundo dourado */}
```

---

## 🔘 Botões

### Tipos de Botão

```tsx
{/* Primário - Roxo com glow */}
<button className="btn btn-primary">Primário</button>

{/* Secundário - Azul escuro com cyan */}
<button className="btn btn-secondary">Secundário</button>

{/* Acento - Dourado */}
<button className="btn btn-accent">Acento</button>

{/* Fantasma - Transparente */}
<button className="btn btn-ghost">Fantasma</button>
```

### Tamanhos

```tsx
<button className="btn btn-primary btn-sm">Pequeno</button>
<button className="btn btn-primary">Médio (padrão)</button>
<button className="btn btn-primary btn-lg">Grande</button>
<button className="btn btn-primary btn-xl">Extragrande</button>
```

### Estado Desabilitado

```tsx
<button className="btn btn-primary" disabled>Desabilitado</button>
```

---

## 🎴 Cards

### Card Básico

```tsx
<div className="card">
  <div className="card-header">
    <h3 className="card-title">Título do Card</h3>
    <p className="card-subtitle">Subtítulo</p>
  </div>
  <div className="card-body">
    <p className="card-text">Conteúdo do card com tema espacial.</p>
  </div>
  <div className="card-footer">
    <button className="btn btn-secondary">Ação</button>
  </div>
</div>
```

### Card com Glow

```tsx
<div className="card card-glow">
  {/* Conteúdo */}
</div>
```

---

## 📝 Inputs

### Input Padrão

```tsx
<div className="input-group">
  <label htmlFor="nome">Nome</label>
  <input 
    type="text" 
    id="nome"
    placeholder="Digite seu nome"
    className="input-sm"
  />
</div>
```

### Tipos de Input

```tsx
{/* Pequeno */}
<input type="text" className="input-sm" />

{/* Médio (padrão) */}
<input type="text" />

{/* Grande */}
<input type="text" className="input-lg" />

{/* Textarea */}
<textarea placeholder="Deixe uma mensagem..."></textarea>

{/* Select */}
<select>
  <option>Opção 1</option>
  <option>Opção 2</option>
</select>
```

---

## 📦 Layout - Grid e Flexbox

### Containers

```tsx
{/* Container responsivo */}
<div className="container-lg">
  Conteúdo centrado, max-width: 1024px
</div>

{/* Container fluid */}
<div className="container-fluid">
  Ocupará 100% da largura
</div>
```

### Flexbox

```tsx
{/* Flex com direção */}
<div className="flex flex-col gap-md">
  {/* Itens em coluna com espaço entre eles */}
</div>

<div className="flex justify-center items-center gap-lg">
  {/* Itens centralizados com espaço entre eles */}
</div>
```

### Grid

```tsx
{/* Grid responsivo */}
<div className="grid grid-cols-3 gap-md">
  <div className="card">Item 1</div>
  <div className="card">Item 2</div>
  <div className="card">Item 3</div>
</div>

{/* Responsivo - 3 colunas em desktop, 2 em tablet, 1 em mobile */}
```

---

## ✨ Animações

### Animações Simples

```tsx
{/* Fade in */}
<div className="fade-in">Elemento fadeIn</div>

{/* Slide in */}
<div className="slide-in-up">Desliza de baixo</div>

{/* Float (flutuação) */}
<div className="float">Elemento flutuando</div>
```

### Animações Espaciais

```tsx
{/* Glow Pulse */}
<button className="btn btn-primary glow-pulse">Botão pulsante</button>

{/* Twinkle (piscar de estrela) */}
<span className="twinkle">✨ Piscante</span>

{/* Rotate */}
<div className="rotate">Girando continuamente</div>

{/* Orbit */}
<div className="orbit">Orbitando em torno</div>
```

### Combinações com Delay

```tsx
<div className="slide-in-up delay-1">Item 1</div>
<div className="slide-in-up delay-2">Item 2</div>
<div className="slide-in-up delay-3">Item 3</div>
```

---

## 🎯 Componentes Avançados

### Badges

```tsx
<span className="badge">Novo</span>
<span className="badge badge-success">Sucesso</span>
<span className="badge badge-warning">Aviso</span>
<span className="badge badge-error">Erro</span>
```

### Tabs

```tsx
<div className="tabs">
  <button className="tab active" data-tab="tab1">Aba 1</button>
  <button className="tab" data-tab="tab2">Aba 2</button>
  <button className="tab" data-tab="tab3">Aba 3</button>
</div>

<div className="tab-content active" id="tab1">
  Conteúdo da aba 1
</div>
<div className="tab-content" id="tab2">
  Conteúdo da aba 2
</div>
```

### Progress Bar

```tsx
<div className="progress">
  <div className="progress-bar" style={{ width: '65%' }}></div>
</div>
```

### Spinner/Loading

```tsx
<span className="spinner"></span>
```

### Tooltip

```tsx
<button className="tooltip" data-tooltip="Clique para explorar">
  Hover me
</button>
```

---

## 🌐 Gradientes Disponíveis

```css
--gradient-space-dark    /* Gradiente escuro cosmológico */
--gradient-space-blue    /* Gradiente azul-roxo-cyan */
--gradient-space-neon    /* Gradiente neon vibrante */
--gradient-nebula        /* Gradiente nebulosa sutil */
```

### Usar Gradientes

```tsx
<div style={{ background: 'var(--gradient-space-neon)' }}>
  Fundo com gradiente neon
</div>
```

---

## 💫 Glow Effects

```css
--glow-cyan          /* Cyan brilho */
--glow-cyan-sm       /* Pequeno */
--glow-cyan-lg       /* Grande */

--glow-purple        /* Roxo brilho */
--glow-purple-sm     /* Pequeno */
--glow-purple-lg     /* Grande */

--glow-gold          /* Dourado brilho */
--glow-gold-sm       /* Pequeno */

--glow-white         /* Branco brilho */
```

---

## 📱 Responsividade

### Breakpoints

```css
640px   - Mobile
768px   - Tablet
1024px  - Desktop
1280px  - Grande Desktop
1536px  - Extra Grande Desktop
```

### Exemplo

```tsx
{/* Automático com CSS Grid */}
<div className="grid grid-cols-6">
  {/* 6 colunas em desktop */}
  {/* 4 colunas em 1024px+ */}
  {/* 3 colunas em 768px+ */}
  {/* 2 colunas em 640px */}
</div>
```

---

## 🎭 Classes Utilitárias

### Visibilidade

```tsx
<div className="hidden">Oculto</div>
<div className="invisible">Invisível mas ocupa espaço</div>
<div className="sr-only">Apenas para leitores de tela</div>
```

### Opacidade

```tsx
<div className="opacity-25">25%</div>
<div className="opacity-50">50%</div>
<div className="opacity-75">75%</div>
<div className="opacity-100">100%</div>
```

### Cores de Texto

```tsx
<p className="text-accent-cyan">Texto cyan</p>
<p className="text-accent-gold">Texto dourado</p>
<p className="text-muted">Texto mutado</p>
```

### Alinhamento

```tsx
<p className="text-left">Esquerda</p>
<p className="text-center">Centro</p>
<p className="text-right">Direita</p>
```

---

## 🔧 Customização

### Adicionar Nova Cor

Edite `variables.css`:

```css
:root {
  --color-custom: #ff00ff;
  --glow-custom: 0 0 10px rgba(255, 0, 255, 0.5);
}
```

Depois use:

```css
.elemento {
  color: var(--color-custom);
  box-shadow: var(--glow-custom);
}
```

### Adicionar Animação Customizada

Edite `animations.css`:

```css
@keyframes myAnimation {
  0% { ... }
  100% { ... }
}

.my-animation {
  animation: myAnimation 1s ease-in-out;
}
```

---

## 📋 Checklist de Implementação

- ✅ Reset de estilos
- ✅ Variáveis CSS customizadas
- ✅ Tipografia completa
- ✅ Sistema de layout (Grid/Flexbox)
- ✅ Componentes (Botões, Cards, Inputs)
- ✅ Animações espaciais
- ✅ Responsive design
- ✅ Dark mode (padrão)
- ✅ Accessibility features
- ✅ Print styles

---

## 🌟 Boas Práticas

1. **Sempre use variáveis CSS** em vez de valores hardcoded
2. **Combine classes** para evitar duplicação: `flex justify-center items-center`
3. **Use animações com moderação** para não sobrecarregar
4. **Testes em mobile** - o design é responsive-first
5. **Prefira Flexbox** para alinhamento e **Grid** para layouts complexos

---

## 🆘 Troubleshooting

### Estilos não carregando?
- Verifique se `main.css` está importado em `index.tsx`
- Clear cache do navegador (Ctrl+Shift+Delete)

### Glow effects não aparecendo?
- Verifique se o navegador suporta box-shadow (Chrome, Firefox, Safari)
- Pode estar oculto por outro elemento com `z-index` maior

### Animações muito rápidas/lentas?
- Ajuste duração em `animations.css`
- Use classes `duration-fast`, `duration-normal`, `duration-slow`

---

## 📚 Recursos Adicionais

- [MDN - CSS Variables](https://developer.mozilla.org/en-US/docs/Web/CSS/--*)
- [MDN - CSS Grid](https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_Grid_Layout)
- [MDN - CSS Flexbox](https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_Flexible_Box_Layout)
- [MDN - CSS Animations](https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_Animations)

---

**Última atualização**: Novembro 2025

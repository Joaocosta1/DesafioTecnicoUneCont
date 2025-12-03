import React, { useEffect, useState } from "react";

const API_BASE_URL = "https://localhost:49854";

function App() {
  const [notas, setNotas] = useState([]);
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState("");

  // filtros / ordenação
  const [filtroCliente, setFiltroCliente] = useState("");
  const [ordenacaoValorAsc, setOrdenacaoValorAsc] = useState(true);

  // campos do formulário
  const [numeroNota, setNumeroNota] = useState("");
  const [cliente, setCliente] = useState("");
  const [valor, setValor] = useState("");
  const [dataEmissao, setDataEmissao] = useState("");

  const [errosForm, setErrosForm] = useState({});

  // Carrega as notas ao iniciar
  useEffect(() => {
    buscarNotas();
  }, []);

  async function buscarNotas() {
    try {
      setCarregando(true);
      setErro("");

      const response = await fetch(`${API_BASE_URL}/api/notas`);
      if (!response.ok) {
        throw new Error("Erro ao buscar notas");
      }

      const data = await response.json();
      setNotas(data);
    } catch (e) {
      console.error(e);
      setErro("Não foi possível carregar as notas.");
    } finally {
      setCarregando(false);
    }
  }

  function validarFormulario() {
    const erros = {};

    if (!numeroNota.trim()) erros.numeroNota = "Número da nota é obrigatório.";
    if (!cliente.trim()) erros.cliente = "Cliente é obrigatório.";
    if (!valor || isNaN(Number(valor)) || Number(valor) <= 0)
      erros.valor = "Valor deve ser maior que zero.";
    if (!dataEmissao) erros.dataEmissao = "Data de emissão é obrigatória.";

    setErrosForm(erros);
    return Object.keys(erros).length === 0;
  }

  async function handleSubmit(e) {
    e.preventDefault();

    if (!validarFormulario()) return;

    const payload = {
      numeroNota,
      cliente,
      valor: Number(valor.replace(",", ".")), // simples conversão
      dataEmissao: dataEmissao, // yyyy-MM-dd → DateTime no .NET entende
    };

    try {
      setCarregando(true);
      setErro("");

      const response = await fetch(`${API_BASE_URL}/api/notas`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(payload),
      });

      if (!response.ok) {
        throw new Error("Erro ao cadastrar nota.");
      }

      // opção 1: recarregar a lista do back
      await buscarNotas();

      // limpa o form
      setNumeroNota("");
      setCliente("");
      setValor("");
      setDataEmissao("");
      setErrosForm({});
    } catch (e) {
      console.error(e);
      setErro("Não foi possível cadastrar a nota.");
    } finally {
      setCarregando(false);
    }
  }

  // Função pra converter "R$ 123,45" em número 123.45
  function parseValorMonetario(valorFormatado) {
    if (!valorFormatado) return 0;
    // remove "R$" e espaços
    let limpo = valorFormatado.replace(/[^\d,.-]/g, "");
    // troca vírgula por ponto (pt-BR)
    limpo = limpo.replace(".", "").replace(",", ".");
    const numero = Number(limpo);
    return isNaN(numero) ? 0 : numero;
  }

  // aplica filtro por cliente
  const notasFiltradas = notas.filter((n) =>
    n.cliente.toLowerCase().includes(filtroCliente.toLowerCase())
  );

  // aplica ordenação por valor
  const notasOrdenadas = [...notasFiltradas].sort((a, b) => {
    const valorA = parseValorMonetario(a.valorFormatado);
    const valorB = parseValorMonetario(b.valorFormatado);

    if (ordenacaoValorAsc) return valorA - valorB;
    return valorB - valorA;
  });

  function alternarOrdenacaoValor() {
    setOrdenacaoValorAsc((prev) => !prev);
  }

  function formatarData(dataStr) {
    if (!dataStr) return "";
    const d = new Date(dataStr);
    if (isNaN(d.getTime())) return dataStr;
    return d.toLocaleDateString("pt-BR");
  }

  return (
    <div style={{ maxWidth: 900, margin: "20px auto", fontFamily: "sans-serif" }}>
      <h1>Cadastro de Notas Fiscais</h1>

      <section
        style={{
          border: "1px solid #ccc",
          borderRadius: 8,
          padding: 16,
          marginBottom: 24,
        }}
      >
        <h2>Nova Nota</h2>
        <form onSubmit={handleSubmit}>
          <div style={{ marginBottom: 8 }}>
            <label>
              Número da Nota:
              <br />
              <input
                type="text"
                value={numeroNota}
                onChange={(e) => setNumeroNota(e.target.value)}
                style={{ width: "100%", padding: 6 }}
              />
            </label>
            {errosForm.numeroNota && (
              <span style={{ color: "red", fontSize: 12 }}>
                {errosForm.numeroNota}
              </span>
            )}
          </div>

          <div style={{ marginBottom: 8 }}>
            <label>
              Cliente:
              <br />
              <input
                type="text"
                value={cliente}
                onChange={(e) => setCliente(e.target.value)}
                style={{ width: "100%", padding: 6 }}
              />
            </label>
            {errosForm.cliente && (
              <span style={{ color: "red", fontSize: 12 }}>
                {errosForm.cliente}
              </span>
            )}
          </div>

          <div style={{ marginBottom: 8 }}>
            <label>
              Valor:
              <br />
              <input
                type="text"
                value={valor}
                onChange={(e) => setValor(e.target.value)}
                style={{ width: "100%", padding: 6 }}
                placeholder="Ex: 100,50"
              />
            </label>
            {errosForm.valor && (
              <span style={{ color: "red", fontSize: 12 }}>
                {errosForm.valor}
              </span>
            )}
          </div>

          <div style={{ marginBottom: 8 }}>
            <label>
              Data de Emissão:
              <br />
              <input
                type="date"
                value={dataEmissao}
                onChange={(e) => setDataEmissao(e.target.value)}
                style={{ padding: 6 }}
              />
            </label>
            {errosForm.dataEmissao && (
              <span style={{ color: "red", fontSize: 12 }}>
                {errosForm.dataEmissao}
              </span>
            )}
          </div>

          <button type="submit" disabled={carregando}>
            {carregando ? "Salvando..." : "Cadastrar"}
          </button>
        </form>
      </section>

      <section
        style={{
          border: "1px solid #ccc",
          borderRadius: 8,
          padding: 16,
        }}
      >
        <h2>Notas Cadastradas</h2>

        <div style={{ marginBottom: 8 }}>
          <label>
            Filtro por Cliente:
            <br />
            <input
              type="text"
              value={filtroCliente}
              onChange={(e) => setFiltroCliente(e.target.value)}
              style={{ width: "100%", padding: 6 }}
              placeholder="Digite o nome do cliente..."
            />
          </label>
        </div>

        <button
          type="button"
          onClick={alternarOrdenacaoValor}
          style={{ marginBottom: 8 }}
        >
          Ordenar por Valor ({ordenacaoValorAsc ? "asc" : "desc"})
        </button>

        {carregando && <p>Carregando...</p>}
        {erro && <p style={{ color: "red" }}>{erro}</p>}

        <table
          style={{
            width: "100%",
            borderCollapse: "collapse",
            marginTop: 8,
            fontSize: 14,
          }}
        >
          <thead>
            <tr>
              <th style={{ borderBottom: "1px solid #ccc", textAlign: "left" }}>
                Número
              </th>
              <th style={{ borderBottom: "1px solid #ccc", textAlign: "left" }}>
                Cliente
              </th>
              <th style={{ borderBottom: "1px solid #ccc", textAlign: "left" }}>
                Valor
              </th>
              <th style={{ borderBottom: "1px solid #ccc", textAlign: "left" }}>
                Emissão
              </th>
              <th style={{ borderBottom: "1px solid #ccc", textAlign: "left" }}>
                Cadastro
              </th>
            </tr>
          </thead>
          <tbody>
            {notasOrdenadas.length === 0 && (
              <tr>
                <td colSpan="5" style={{ padding: 8 }}>
                  Nenhuma nota cadastrada.
                </td>
              </tr>
            )}

            {notasOrdenadas.map((n, index) => (
              <tr key={index}>
                <td style={{ padding: 4 }}>{n.numeroNota}</td>
                <td style={{ padding: 4 }}>{n.cliente}</td>
                <td style={{ padding: 4 }}>{n.valorFormatado}</td>
                <td style={{ padding: 4 }}>{formatarData(n.dataEmissao)}</td>
                <td style={{ padding: 4 }}>{formatarData(n.dataCadastro)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </section>
    </div>
  );
}

export default App;

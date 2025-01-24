import React, {useState, useEffect} from 'react';
import './App.css';

import 'bootstrap/dist/css/bootstrap.min.css';
import axios from 'axios';
import {Modal, ModalBody, ModalFooter, ModalHeader} from 'reactstrap';
import logoCadastro from './assets/cadastro.png';

function App() {

// Define a URL base para a API
const baseUrl = "https://localhost:7266/api/alunos";

// Define um estado para armazenar os dados recebidos da API
const [data, setData] = useState([]);
  
// Define o estado inicial para 'alunoSelecionado' com propriedades vazias
const [alunoSelecionado, setAlunoSelecionado] = useState({
  id: '',
  nome: '',
  email: '',
  idade: ''
});

const [modalIncluir, setModalIncluir]=useState(false);

const abrirFecharModalIncluir=()=>{
  setModalIncluir(!modalIncluir);
}

// Função que lida com mudanças nos campos de entrada
const handleChange = e => {
  // Extrai o nome e valor do campo que disparou o evento
  const { name, value } = e.target;
  
  // Atualiza o estado 'alunoSelecionado' com o novo valor do campo correspondente
  setAlunoSelecionado({
    ...alunoSelecionado, // Mantém os valores atuais das outras propriedades
    [name]: value // Atualiza a propriedade correspondente ao nome do campo
  });
  
  // Loga o estado atualizado no console (pode não mostrar a atualização imediatamente devido à natureza assíncrona do setState)
  console.log(alunoSelecionado);
}

// Função assíncrona para fazer uma requisição GET à API
const pedidoGet = async() => {
  // Faz a requisição GET à API usando axios
  await axios.get(baseUrl)
    .then(response => {
      // Se a requisição for bem-sucedida, armazena os dados recebidos no estado
      setData(response.data);
    }).catch(error => {
      // Se ocorrer um erro, exibe o erro no console
      console.log(error);
    });
}

const pedidoPost=async()=>{
  // Faz a requisição POST à API usando axios
  delete alunoSelecionado.id;
  alunoSelecionado.idade=parseInt(alunoSelecionado.idade);
  await axios.post(baseUrl,alunoSelecionado)
  .then(response=>{
    setData(data.concat(response.data));
    abrirFecharModalIncluir();
  }).catch(error=>{
    console.log(error);
  })
}

// useEffect é usado para executar a função pedidoGet quando o componente é montado
useEffect(() => {
  pedidoGet();
}); // O array vazio [] significa que o efeito só será executado uma vez, após a montagem do componente

  return (
    <div className="App">
      <br/>
      <h3> Cadastro de Alunos</h3>
      <header>
          <img src={logoCadastro} alt='Cadastro' />
          <button onClick={()=>abrirFecharModalIncluir()} className="btn btn-success">Incluir Novo Aluno</button>
      </header> 
      <table className="table table-bordered">
        <thead>
          <tr>
            <th>Id</th>
            <th>Nome</th>
            <th>Email</th>
            <th>Idade</th>
            <th>Operação</th>
          </tr>
        </thead>
        <tbody>
          {data.map(aluno =>(
            <tr key={aluno.id}>
              <td>{aluno.id}</td>
              <td>{aluno.nome}</td>
              <td>{aluno.email}</td>
              <td>{aluno.idade}</td>
              <td>
                <button className="btn btn-primary">Editar</button> {" "}
                <button className="btn btn-danger">Excluir</button>
              </td>
            </tr>
          ))}
        </tbody>
        </table>
        <Modal isOpen={modalIncluir}>
          <ModalHeader>Incluir Alunos</ModalHeader>
          <ModalBody>
            <div className="form-group">
              <label>Nome: </label>
              <br/>
              <input type="text" className="form-control" name="nome" onChange={handleChange}/>
              <br/>
              <label>Email: </label>
              <br/>
              <input type="text" className="form-control" name="email" onChange={handleChange}/>
              <br/>
              <label>Idade: </label>
              <br/>
              <input type="text" className="form-control" name="idade" onChange={handleChange}/>
              <br/>  
            </div>
          </ModalBody>
          <ModalFooter>
            <button className="btn btn-primary" onClick={()=> pedidoPost()}>Incluir</button>{" "}
            <button className="btn btn-danger" onClick={()=> abrirFecharModalIncluir()}>Cancelar</button>
          </ModalFooter>
        </Modal>

    </div>
  );
}

export default App;

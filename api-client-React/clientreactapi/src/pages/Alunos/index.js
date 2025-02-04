import React, {useState, useEffect} from 'react';
import api from '../../services/api';
import { Link, useHistory } from 'react-router-dom';
import './styles.css';
import logoCadastro from '../../assets/cadastro.png';

import { FiXCircle, FiEdit, FiUserX } from 'react-icons/fi';


export default function Alunos() {
 
    //filtrar dados
    const [searchInput, setSearchInput] = useState('');
    const [filtro, setFiltro] = useState([]);

    const[alunos, setAlunos] = useState([]);

    const email = localStorage.getItem('email');
    const token = localStorage.getItem('token');

    const history = useHistory();

    const authorization = 
    {
        headers :
        {
            Authorization : `Bearer ${token}`
        }
    }

    const searchAlunos = (searchValue) => 
    {
        setSearchInput(searchValue);
        if(searchInput !== '')
        {
            const filtroAlunos = alunos.filter((item) => 
            {
                return Object.values(item).join('').toLowerCase().includes(searchInput.toLowerCase());
            });
            setFiltro(filtroAlunos);
        }
        else
        {
            setFiltro(alunos);
        }
    } 

    useEffect(() => 
    {
        api.get('/api/Alunos', authorization).then(
            response => 
            {
                setAlunos(response.data);
            }, token) 
    })

    async function logout()
    {
        try
        {
            localStorage.clear();
            localStorage.setItem('token', '');
            authorization.headers = '';
            history.push('/');
        }
        catch (error)
        {
            alert('Erro ao realizar logout, tente novamente' + error);
        }
    }

    async function editAluno(id)
    {
        try
        {
            history.push(`/aluno/novo/${id}`);
        }
        catch (error)
        {
            alert('Erro ao editar aluno, tente novamente' + error);
        }
    }

    return(
        <div className="alunos-container">
            <header>
                <img class="logo" src={logoCadastro} alt="Cadastro"/>  
                <span> Bem-Vindo,<strong>{email}!</strong></span>
                <br></br>
                <Link class="buttonAluno" to="aluno/novo/0">Novo Aluno</Link>
                <button onClick={logout} class="buttonX" type="button">
                    <FiXCircle size={35} color="#17202a"/>
                </button>
            </header>
            <form>
                <input class="nomeClass" type="text" placeholder="Filtrar por nome..." onChange={(e) => searchAlunos(e.target.value)}/>
            </form>
            <h1>Relação de Alunos</h1>  
            {searchInput.length > 1 ? (
            <ul class="alunos-list">
                {filtro.map(aluno => (
                    <li key={aluno.id}>
                        <b>Nome: </b>{aluno.nome}<br></br>
                        <b>Email: </b>{aluno.email}<br></br>
                        <b>Idade: </b>{aluno.idade}<br></br>                 
        
                        <button onClick={()=> editAluno(aluno.id)} type="button">
                            <FiEdit size="25" color="#17202a"/>
                        </button>
                        <button type="button">
                            <FiUserX size="25" color="#17202a"/>
                        </button>
                    </li>
                ))}
            </ul> 
            ) : (
            <ul class="alunos-list"> 
                {alunos.map(aluno =>(
                    <li key={aluno.id}>
                    <b>Nome: </b>{aluno.nome}<br></br>
                    <b>Email: </b>{aluno.email}<br></br>
                    <b>Idade: </b>{aluno.idade}<br></br>                 

                    <button onClick={()=> editAluno(aluno.id)} type="button">
                        <FiEdit size="25" color="#17202a"/>
                    </button>
                    <button type="button">
                        <FiUserX size="25" color="#17202a"/>
                    </button>
                    </li>
                    ))}
            </ul>   
            )} 
        </div>
    )
}
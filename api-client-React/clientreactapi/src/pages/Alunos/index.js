import React from 'react';
import { Link } from 'react-router-dom';
import './styles.css';
import logoCadastro from '../../assets/cadastro.png';

import { FiXCircle, FiEdit, FiUserX } from 'react-icons/fi';


export default function Alunos() {
    return(
        <div className="alunos-container">
            <header>
                <img class="logo" src={logoCadastro} alt="Cadastro"/>  
                <span> Bem-Vindo, <strong>Foqs</strong>!</span>
                <br></br>
                <Link class="button" to="aluno/novo/0">Novo Aluno</Link>
                <button class="buttonX" type="button">
                    <FiXCircle size={35} color="#17202a"/>
                </button>
            </header>
            <form>
                <input type="text" placeholder="Nome"/>
                <button type="button" class="button">
                    Filtrar aluno por nome (parcial)
                </button>
            </form>
            <h1>Relação de Alunos</h1>  
            <ul class="alunos-list">
                <li>
                <b>Nome: </b> Paulo<br></br>
                <b>Email: </b> paulo@email.com<br></br>
                <b>Idade: </b> 22<br></br>
                <button type="button">
                    <FiEdit size="25" color="#17202a"/>
                </button>
                <button type="button">
                    <FiUserX size="25" color="#17202a"/>
                </button>
                </li>

                <li>
                <b>Nome: </b> Paulo<br></br>
                <b>Email: </b> paulo@email.com<br></br>
                <b>Idade: </b> 22<br></br>
                <button type="button">
                    <FiEdit size="25" color="#17202a"/>
                </button>
                <button type="button">
                    <FiUserX size="25" color="#17202a"/>
                </button>
                </li>

                <li>
                <b>Nome: </b> Paulo<br></br>
                <b>Email: </b> paulo@email.com<br></br>
                <b>Idade: </b> 22<br></br>
                <button type="button">
                    <FiEdit size="25" color="#17202a"/>
                </button>
                <button type="button">
                    <FiUserX size="25" color="#17202a"/>
                </button>
                </li>

                
            </ul>
            </div>
    )
}
import React from 'react'; 
import './styles.css';
import { Link, useParams } from 'react-router-dom';
import { FiCornerDownLeft, FiUserPlus } from 'react-icons/fi';

export default function NovoAluno()
{ 
    const {alunoId} = useParams();  

    return(
        <div class="newAluno-container">
            <div class="content"> 
                <section class="form"></section>
                <FiUserPlus size="105" color="#17202a"/>
                <h1>{alunoId === '0'?'Incluir Novo Aluno' : 'Atualizar Aluno'}</h1>
                <Link class="back-link" to="/alunos">
                    <FiCornerDownLeft size="25" color="#17202a"/>
                    Retornar
                </Link>
                <form>
                    <input placeholder="Nome" />
                    <input placeholder="Email" />
                    <input placeholder="Idade" />
                    <button class="button" type="submit">{alunoId === '0'?'Incluir' : 'Atualizar'}</button>
                </form>
            </div>
        </div>
    );
}
import React, {useEffect, useState} from 'react'; 
import './styles.css';
import { Link, useParams, useHistory } from 'react-router-dom';
import { FiCornerDownLeft, FiUserPlus } from 'react-icons/fi';
import api from '../../services/api';

export default function NovoAluno()
{ 
    const[id, setId] = useState(null);
    const[nome, setNome] = useState('');
    const[email, setEmail] = useState('');
    const[idade, setIdade] = useState(0);

    const {alunoId} = useParams();  

    const history = useHistory();

    const token = localStorage.getItem('token');

    const authorization = 
    {
        headers :
        {
            Authorization : `Bearer ${token}`
        }
    }

    useEffect(() =>
    {
        if(alunoId === '0')
        {
            return;
        }
        else
        {
            loadAluno();
        }
    }, alunoId)

    async function loadAluno()
    {
        try
        {
            const response = await api.get(`api/alunos/${alunoId}`, authorization);

            setId(response.data.id);
            setNome(response.data.nome);
            setEmail(response.data.email);
            setIdade(response.data.idade);
        }
        catch (error)
        {
            alert('Erro ao carregar aluno, tente novamente' + error);
            history.push('/alunos');
        }
    }

    async function saveOrUpdate(event)
    {
        event.preventDefault();

        const data = 
        {
            nome,
            email,
            idade
        }

        try
        {
            if(alunoId === '0') //se o id for 0, é pra postar
            {
                await api.post('api/alunos', data, authorization);
            }
            else
            {
                data.id=id; //se o id for diferente de 0, é pra editar
                await api.put(`api/alunos/${alunoId}`, data, authorization);
            }
        }
        catch (error)
        {
            alert('Erro ao salvar aluno, tente novamente' + error);
        }
        history.push('/alunos');
    }

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

                <form onSubmit={saveOrUpdate}>
                    <input placeholder="Nome" 
                        value={nome}
                        onChange={e=> setNome(e.target.value)}
                    />
                    <input placeholder="Email" 
                        value={email}
                        onChange={e=> setEmail(e.target.value)}
                    />
                    <input placeholder="Idade" 
                        value={idade}
                        onChange={e=> setIdade(e.target.value)}
                    />
                    <button class="button" type="submit">{alunoId === '0'?'Incluir' : 'Atualizar'}</button>
                </form>
            </div>
        </div>
    );
}
import React from 'react'; 
import { BrowserRouter, Switch, Route } from 'react-router-dom';
import Login from './pages/Login';  
import Alunos from './pages/Alunos';
import NovoAluno from './pages/novoAluno';

export default function AppRoutes()
{
    return (
        <BrowserRouter>
            <Switch>
                <Route path="/" exact component={Login} />
                <Route path="/alunos" component={Alunos} />
                <Route path="/aluno/novo/:alunoId" component={NovoAluno} />
            </Switch>
        </BrowserRouter>
    );
}
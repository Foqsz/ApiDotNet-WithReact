import React from 'react'; 
import { BrowserRouter, Switch, Route } from 'react-router-dom';
import Login from './pages/Login';  
import Alunos from './pages/Alunos';

export default function AppRoutes()
{
    return (
        <BrowserRouter>
            <Switch>
                <Route path="/" exact component={Login} />
                <Route path="/alunos" component={Alunos} />
            </Switch>
        </BrowserRouter>
    );
}
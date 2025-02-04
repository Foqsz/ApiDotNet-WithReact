import React, {useState} from 'react';
import './styles.css';
import api from '../../services/api';
import {useHistory} from 'react-router-dom';

import logoImage from '../../assets/logo.png';

export default function Login()
{
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const history = useHistory();

    async function login(event)
    {
        event.preventDefault();

        const data = { email, password };

        try
        {
            const response = await api.post('/api/Account/LoginUser', data);

            localStorage.setItem('email', email);
            localStorage.setItem('token', response.data.token);
            localStorage.setItem('expiration', response.data.expiration);

            history.push('/alunos');
        }
        catch(error)
        {
            alert('Falha no login, tente novamente.' + error);
        }
    }

    return(
        <div className="login-container">
            <section className="form">
                <img src={logoImage} alt="login" id="img1" style={{ width: '150px', height: 'auto' }} />
                <form onSubmit={login}>
                    <h1>Cadastro de Alunos</h1>
                    <h2>Acesso restrito a funcionários.</h2>

                    <input placeholder="Email"
                        value={email}
                        onChange={e=>setEmail(e.target.value)}
                    />
                    
                    <input type="password" placeholder="Password" 
                        value={password}
                        onChange={e=>setPassword(e.target.value)}
                    />

                    <button className="button" type="submit">Login</button>

                </form>
            </section>
        </div>
    )
}
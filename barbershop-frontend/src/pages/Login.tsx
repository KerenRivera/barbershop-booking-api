import { useState } from 'react';
import { Box, TextField, Button, Typography, Paper, Alert } from '@mui/material';

import api from '../services/api';

export default function Login() {
  const [isRegister, setIsRegister] = useState(false);
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async () => {
    try {
      setError('');
      setMessage('');
      if (isRegister) {
        await api.post('/Auth/register', { name, email, password });
        setMessage('Registro exitoso. Ahora puedes iniciar sesión.');
        setIsRegister(false);
      } else {
        const res = await api.post('/Auth/login', { email, password });
        localStorage.setItem('token', res.data.token);
        setMessage('Login exitoso.');
        window.location.href = '/services';
      }
    } catch (err: any) {
      setError(err.response?.data || 'Error en la solicitud');
    }
  };

  return (
    <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '100vh', bgcolor: '#f5f5f5' }}>
      <Paper sx={{ p: 4, width: 400 }}>
        <Typography variant="h4" align="center" gutterBottom>
          BarberShop Booking
        </Typography>
        <Typography variant="h6" align="center" gutterBottom>
          {isRegister ? 'Registro' : 'Iniciar Sesión'}
        </Typography>
        {message && <Alert severity="success" sx={{ mb: 2 }}>{message}</Alert>}
        {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
        {isRegister && (
          <TextField fullWidth label="Nombre" margin="normal" value={name} onChange={e => setName(e.target.value)} />
        )}
        <TextField fullWidth label="Correo" margin="normal" value={email} onChange={e => setEmail(e.target.value)} />
        <TextField fullWidth label="Contraseña" type="password" margin="normal" value={password} onChange={e => setPassword(e.target.value)} />
        <Button fullWidth variant="contained" sx={{ mt: 2 }} onClick={handleSubmit}>
          {isRegister ? 'Registrarse' : 'Iniciar Sesión'}
        </Button>
        <Button fullWidth sx={{ mt: 1 }} onClick={() => setIsRegister(!isRegister)}>
          {isRegister ? 'Ya tengo cuenta' : 'Crear cuenta'}
        </Button>
      </Paper>
    </Box>
  );
}
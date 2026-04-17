import { useState, useEffect } from 'react';
import { Box, Typography, TextField, Button, MenuItem, AppBar, Toolbar, Alert, Paper } from '@mui/material';
import api from '../services/api';

export default function Book() {
  const [services, setServices] = useState<any[]>([]);
  const [barbers, setBarbers] = useState<any[]>([]);
  const [serviceId, setServiceId] = useState('');
  const [barberId, setBarberId] = useState('');
  const [dateTime, setDateTime] = useState('');
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');

  useEffect(() => {
    api.get('/Services').then(res => setServices(res.data));
    api.get('/Barbers').then(res => setBarbers(res.data));
  }, []);

  const handleBook = async () => {
    try {
      setError('');
      setMessage('');
      await api.post('/Appointments', {
        serviceId: parseInt(serviceId),
        barberId: parseInt(barberId),
        dateTime: dateTime,
      });
      setMessage('¡Cita agendada exitosamente!');
    } catch (err: any) {
      setError(err.response?.data || 'Error al agendar la cita');
    }
  };

  return (
    <Box>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" sx={{ flexGrow: 1 }}>BarberShop Booking</Typography>
          <Button color="inherit" href="/services">Servicios</Button>
          <Button color="inherit" href="/book">Agendar</Button>
          <Button color="inherit" href="/">Salir</Button>
        </Toolbar>
      </AppBar>
      <Box sx={{ p: 3, display: 'flex', justifyContent: 'center' }}>
        <Paper sx={{ p: 4, width: 500 }}>
          <Typography variant="h4" gutterBottom>Agendar Cita</Typography>
          {message && <Alert severity="success" sx={{ mb: 2 }}>{message}</Alert>}
          {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
          <TextField
            select fullWidth label="Servicio" margin="normal"
            value={serviceId} onChange={e => setServiceId(e.target.value)}
          >
            {services.map(s => (
              <MenuItem key={s.id} value={s.id}>{s.name} - RD$ {s.price}</MenuItem>
            ))}
          </TextField>
          <TextField
            select fullWidth label="Barbero" margin="normal"
            value={barberId} onChange={e => setBarberId(e.target.value)}
          >
            {barbers.map(b => (
              <MenuItem key={b.id} value={b.id}>{b.name}</MenuItem>
            ))}
          </TextField>
          <TextField
            fullWidth label="Fecha y hora" type="datetime-local" margin="normal"
            value={dateTime} onChange={e => setDateTime(e.target.value)}
            InputLabelProps={{ shrink: true }}
          />
          <Button fullWidth variant="contained" sx={{ mt: 2 }} onClick={handleBook}>
            Confirmar Cita
          </Button>
        </Paper>
      </Box>
    </Box>
  );
}
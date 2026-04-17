import { useState, useEffect } from 'react';
import { Box, Typography, Card, CardContent, Grid, AppBar, Toolbar, Button } from '@mui/material';
import api from '../services/api';

export default function Services() {
  const [services, setServices] = useState<any[]>([]);

  useEffect(() => {
    api.get('/Services').then(res => setServices(res.data));
  }, []);

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
      <Box sx={{ p: 3 }}>
        <Typography variant="h4" gutterBottom>Nuestros Servicios</Typography>
        <Grid container spacing={3}>
          {services.map((s: any) => (
            <Grid key={s.id} item xs={12} sm={6} md={4}>
              <Card>
                <CardContent>
                  <Typography variant="h6">{s.name}</Typography>
                  <Typography color="text.secondary">{s.description}</Typography>
                  <Typography variant="h5" color="primary" sx={{ mt: 1 }}>RD$ {s.price}</Typography>
                  <Typography variant="body2">{s.durationMinutes} minutos</Typography>
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Box>
    </Box>
  );
}
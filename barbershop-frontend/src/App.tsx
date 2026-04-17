import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Login from './pages/Login';
import Services from './pages/Services';
import Book from './pages/Book';

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/services" element={<Services />} />
        <Route path="/book" element={<Book />} />
      </Routes>
    </BrowserRouter>
  );
}

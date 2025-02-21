import { useState } from "react";
import { Form, Button } from "react-bootstrap";
import { Eye, EyeOff } from "lucide-react";
import "./SignPage.css";
import { isValidEmail } from "../services/textFunctions";
import api from "../api";
import { useNavigate } from "react-router-dom";

const SignInPage = () => {
  const [formData, setFormData] = useState({ email: "", password: "" });
  const [errors, setErrors] = useState({});
  const [showPassword, setShowPassword] = useState(false);
  const [showPasswordEyes, setShowPasswordEyes] = useState(true);
  const navigate = useNavigate();

  const handleInputChange = (field, value) => {
    setFormData({ ...formData, [field]: value });
    setErrors((prevErrors) => ({ ...prevErrors, [field]: "" }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    resetErrors();

    if (!validateForm()) {
      return;
    }

    try {
      const user = {
        email: formData.email,
        password: formData.password
      };

      const response = await api.post("/api/auth/login", user);

      localStorage.setItem("token", response.data.token);

      navigate("/");

    } catch (err) {
      setErrors({ general: "Неверный логин или пароль!" });
    }
  };

  const validateForm = () => {
    const newErrors = {};
    if(!formData.email) {
      newErrors.email = "Укажите вашу почту!";
    }
    if(formData.email && !isValidEmail(formData.email)) {
      newErrors.email = "Неверный формат почты!";
    }
    if(!formData.password) {
      newErrors.password = "Введите пароль!";
      setShowPasswordEyes(false);
    }
    if(formData.password && formData.password.length < 4) {
      newErrors.password = "Пароль слишком короткий!";
      setShowPasswordEyes(false);
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const resetErrors = () => {
    setErrors({});
    setShowPasswordEyes(true);
  };

  return (
    <div className="login-page">
      <header className="login-header">
        <h2>Вход</h2>
      </header>
      <div className="login-container">
        <Form onSubmit={handleSubmit}>
          <Form.Group className="sign-left-group">
            <Form.Label>Электронная почта</Form.Label>
            <Form.Control 
              className="control-input" 
              type="email"
              value={formData.email}
              onChange={(e) => handleInputChange("email", e.target.value)}
              isInvalid={!!errors.email}
              />
              {errors.email && <Form.Text className="text-danger">{errors.email}</Form.Text>}
          </Form.Group>

          <Form.Group className="password-group sign-left-group">
            <Form.Label>Пароль</Form.Label>
            <Form.Control
              className="control-input"
              type={showPassword ? "text" : "password"}
              value={formData.password}
              onChange={(e) => {
                handleInputChange("password", e.target.value);
                setShowPasswordEyes(true);
              }}
              isInvalid={!!errors.password}
              />
              {errors.password && <Form.Text className="text-danger">{errors.password}</Form.Text>}
              {errors.general && <Form.Text className="text-danger">{errors.general}</Form.Text>}
            {showPasswordEyes && (showPassword ? (
              <EyeOff className="eye-icon" onClick={() => setShowPassword(false)} />
            ) : (
              <Eye className="eye-icon" onClick={() => setShowPassword(true)} />
            ))}
          </Form.Group>
          <Button type="submit" className="apply-button sign-button">Войти</Button>
        </Form>
      </div>
    </div>
  );
};

export default SignInPage;

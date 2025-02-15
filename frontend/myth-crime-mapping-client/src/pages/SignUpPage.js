import { useState } from "react";
import { Form, Button } from "react-bootstrap";
import { Eye, EyeOff } from "lucide-react";
import "./SignPage.css";
import { isValidEmail } from "../services/textFunctions";
import api from "../api";

const SignUpPage = () => {
  const [formData, setFormData] = useState({
    name: "",
    surname: "",
    patronymic: "",
    position: "",
    email: "",
    password: "",
    confirmPassword: "",
  });
  const [errors, setErrors] = useState({});
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [showPasswordEyes, setShowPasswordEyes] = useState(true);
  const [showConfirmPasswordEyes, setShowConfirmPasswordEyes] = useState(true);

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
      // TODO
      const user = {
        name: formData.name,
        surname: formData.surname,
        patronymic: formData.patronymic,
        position: formData.position,
        email: formData.email,
        password: formData.password
      };

      const response = await api.post("/api/auth/signup", user);

      console.log(response.data);
      

    } catch (err) {
      console.log(err);
    }
  };

  const validateForm = () => {
    const newErrors = {};
    if (!formData.surname) {
      newErrors.surname = "Укажите вашу фамилию!";
    }
    if (!formData.name) {
      newErrors.name = "Укажите ваше имя!";
    }
    if (!formData.position) {
      newErrors.position = "Укажите вашу должность!";
    }
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
    if(!formData.confirmPassword) {
      newErrors.confirmPassword = "Подтвердите пароль!";
      setShowConfirmPasswordEyes(false);
    }
    if(formData.password && formData.confirmPassword 
      && formData.password!==formData.confirmPassword) {
      newErrors.password = "Пароли не совпадают!";
      newErrors.confirmPassword = "Пароли не совпадают!";
      setShowPasswordEyes(false);
      setShowConfirmPasswordEyes(false);
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
    setShowConfirmPasswordEyes(true);
  };

  return (
    <div className="login-page">
      <header className="login-header">
        <h2>Регистрация</h2>
      </header>
      <div className="login-container">
        <Form onSubmit={handleSubmit}>
          <Form.Group className="sign-left-group">
            <Form.Label>Фамилия</Form.Label>
            <Form.Control
              className="control-input"
              type="text"
              value={formData.surname}
              onChange={(e) => handleInputChange("surname", e.target.value)}
              isInvalid={!!errors.surname}
            />
            {errors.surname && <Form.Text className="text-danger">{errors.surname}</Form.Text>}
          </Form.Group>

          <Form.Group className="sign-left-group">
            <Form.Label>Имя</Form.Label>
            <Form.Control
              className="control-input"
              type="text"
              value={formData.name}
              onChange={(e) => handleInputChange("name", e.target.value)}
              isInvalid={!!errors.name}
              />
              {errors.name && <Form.Text className="text-danger">{errors.name}</Form.Text>}
          </Form.Group>

          <Form.Group className="sign-left-group">
            <Form.Label>Отчество (необязательно)</Form.Label>
            <Form.Control
              className="control-input"
              type="text"
              value={formData.patronymic}
              onChange={(e) => handleInputChange("patronymic", e.target.value)}
              isInvalid={!!errors.patronymic}
              />
              {errors.patronymic && <Form.Text className="text-danger">{errors.patronymic}</Form.Text>}
          </Form.Group>

          <Form.Group className="sign-left-group">
            <Form.Label>Должность</Form.Label>
            <Form.Control
              className="control-input"
              type="text"
              value={formData.position}
              onChange={(e) => handleInputChange("position", e.target.value)}
              isInvalid={!!errors.position}
              />
              {errors.position && <Form.Text className="text-danger">{errors.position}</Form.Text>}
          </Form.Group>

          <Form.Group className="sign-left-group">
            <Form.Label>Электронная почта</Form.Label>
            <Form.Control 
              className="control-input" 
              type="text"
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

            {showPasswordEyes && (showPassword ? (
              <EyeOff className="eye-icon" onClick={() => setShowPassword(false)} />
            ) : (
              <Eye className="eye-icon" onClick={() => setShowPassword(true)} />
            ))}
          </Form.Group>

          <Form.Group className="password-group sign-left-group">
            <Form.Label>Подтвердите пароль</Form.Label>
            <Form.Control
              className="control-input"
              type={showConfirmPassword ? "text" : "password"}
              value={formData.confirmPassword}
              onChange={(e) => {
                handleInputChange("confirmPassword", e.target.value);
                setShowConfirmPasswordEyes(true);
              }}
              isInvalid={!!errors.confirmPassword}
              />
              {errors.confirmPassword && <Form.Text className="text-danger">{errors.confirmPassword}</Form.Text>}
            {showConfirmPasswordEyes && (showConfirmPassword ? (
              <EyeOff className="eye-icon" onClick={() => setShowConfirmPassword(false)} />
            ) : (
              <Eye className="eye-icon" onClick={() => setShowConfirmPassword(true)} />
            ))}
          </Form.Group>
          <Button type="submit" className="apply-button sign-button">Зарегистрироваться</Button>
        </Form>
      </div>
    </div>
  );
};

export default SignUpPage;

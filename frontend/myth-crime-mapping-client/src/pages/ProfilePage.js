import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Eye, EyeOff } from "lucide-react";
import { Modal, Form } from "react-bootstrap";
import api from "../api";
import { getUserId, removeToken } from "../services/authFunctions";
import "./ProfilePage.css";
import { isValidEmail } from "../services/textFunctions";

const ProfilePage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [user, setUser] = useState(null);
  const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);
  const [showEditPassword, setEditPassword] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [formData, setFormData] = useState({
    name: "",
    surname: "",
    patronymic: "",
    position: "",
    email: "",
  });
  const [passwordForm, setPasswordForm] = useState({
    code: "",
    password: "",
    confirmPassword: ""
  });
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [showPasswordEyes, setShowPasswordEyes] = useState(true);
  const [showConfirmPasswordEyes, setShowConfirmPasswordEyes] = useState(true);
  const [errors, setErrors] = useState({});

  const token = localStorage.getItem("token");
  const currentUserId = getUserId();

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await api.get(`/api/users/${id}`);
        setUser(response.data);
        setFormData(response.data);
      } catch (error) {
        console.error("Ошибка загрузки данных профиля:", error);
      }
    };

    fetchUser();
  }, [id]);

  const handleSave = async () => {
    resetErrors();
    const valid = validateForm();
    if (!valid) {
      console.log("No valid");
      return;
    }

    const payload = {
      name: formData.name || null,
      surname: formData.surname || null,
      patronymic: formData.patronymic || null,
      position: formData.position || null,
      email: formData.email || null
    }

    try {
      await api.patch(`/api/users/${id}`, payload, {
        headers: { Authorization: `Bearer ${token}` },
      });
      setUser(formData);
      setIsEditing(false);
    } catch (error) {
      console.error("Ошибка обновления профиля:", error);
    }
  };

  const handleDelete = async () => {
    try {
      await api.delete(`/api/users/${id}`,
        {
          headers: { Authorization: `Bearer ${token}` }
        }
      );
    } catch(error) {
      console.error("Error deleting wanted person:", error.response);
    }
  };

  const handleEding = () => {
    resetErrors();
    setFormData(user);
    setIsEditing(true);
  }

  const canselEding = () => {
    setIsEditing(false);
    resetErrors();
  }

  const handleInputChange = (field, value) => {
    setFormData({ ...formData, [field]: value });
  };

  const handleLogout = () => {
    removeToken();
    navigate("/login");
  };

  const handleDeleteClick = () => {
    setShowDeleteConfirm(true);
  };

  const cancelDeleteClick = () => {
    setShowDeleteConfirm(false);
  };

  const confirmDeleteClick = async () => {
    await handleDelete();
    setShowDeleteConfirm(false);
    navigate("/login");
  };

  const handleEditPassword = async() => {
    try {
      const response = await api.post("/api/auth/code", getUserId(),
        {
          headers: { 
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json' 
          }
        }
      )
      console.log(response.data);
      resetErrors();
      setPasswordForm({
        code: "",
        password: "",
        confirmPassword: ""
      });
      setEditPassword(true);
    }
    catch (err) {
      console.log("Error:: " + err);
    }

  }

  const cancelEditPassword = () => {
    setEditPassword(false);
    setPasswordForm({
      code: "",
      password: "",
      confirmPassword: ""
    });
    resetErrors();
  }

  const confirmEditPasswordClick = async() => {
    resetErrors();
    const valid = validateChangePasswordForm();
    if (!valid) {
      console.log("No valid");
      return;
    }
    
    const data = {
      userId: getUserId(),
      password: passwordForm.password,
      code: passwordForm.code
    };
    console.log(data);
    try {
      const response = await api.post("/api/auth/changepassword", data,
        {
          headers: { 
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json' 
          }
        }
      );
      console.log(response.data);

      setEditPassword(false);
    } catch(err) {
      console.log(err);
      setErrors({code: "Неверный код подтверждения!"});
    }
  }

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

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const validateChangePasswordForm = () => {
    const newErrors = {};
    if (!passwordForm.code) {
      newErrors.code = "Введите код подтверждения!";
    }
    if(!passwordForm.password) {
      newErrors.password = "Введите пароль!";
      setShowPasswordEyes(false);
    }
    if(!passwordForm.confirmPassword) {
      newErrors.confirmPassword = "Подтвердите пароль!";
      setShowConfirmPasswordEyes(false);
    }
    if(passwordForm.password && passwordForm.confirmPassword 
      && passwordForm.password!==passwordForm.confirmPassword) {
      newErrors.password = "Пароли не совпадают!";
      newErrors.confirmPassword = "Пароли не совпадают!";
      setShowPasswordEyes(false);
      setShowConfirmPasswordEyes(false);
    }
    if(passwordForm.password && passwordForm.password.length < 4) {
      newErrors.password = "Пароль слишком короткий!";
      setShowPasswordEyes(false);
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  }

  const resetErrors = () => {
    setErrors({});
    setShowPasswordEyes(true);
    setShowConfirmPasswordEyes(true);
  };

  if (!user) return <p>Загрузка...</p>;

  return (
    <div className="profile-page">
      <header className="profile-header">
        <h2>Профиль</h2>
      </header>
      <div className="profile-content">
        <div className="profile-info">
          <img src="/icons/profile-img.jpg" alt="profile-image"/>
          <div className="profile-text">
            <p><strong>Фамилия:</strong> {user.surname}</p>
            <p><strong>Имя:</strong> {user.name}</p>
            <p><strong>Отчество:</strong> {user.patronymic}</p>
            <p><strong>Должность:</strong> {user.position}</p>
            <p><strong>Email:</strong> {user.email}</p>
          </div>
        </div>


        {currentUserId === id && 
          <div className="profile-button-group">
            <button className="apply-button" onClick={handleEding}>Редактировать</button>
            <button className="reset-button" onClick={handleDeleteClick}>Удалить профиль</button>
            <button className="apply-button" onClick={handleEditPassword}>Изменить пароль</button>
            <button className="cansel-button" onClick={handleLogout}>Выйти</button>
          </div>
        }
      </div>

    <Modal show={isEditing} onHide={canselEding}>
      <Modal.Header>
      <Modal.Title>Редактирование профиля</Modal.Title>
      </Modal.Header>
      <Modal.Body>
          <Form.Group>
            <Form.Label>Фамилия</Form.Label>
            <Form.Control
              type="text"
              value={formData.surname}
              onChange={(e) => handleInputChange("surname", e.target.value)}
              isInvalid={!!errors.surname}
            />
            {errors.surname && <Form.Text className="text-danger">{errors.surname}<br/></Form.Text>}
          </Form.Group>
          <Form.Group>
            <Form.Label>Имя</Form.Label>
            <Form.Control
              type="text"
              value={formData.name}
              onChange={(e) => handleInputChange("name", e.target.value)}
              isInvalid={!!errors.name}
            />
            {errors.name && <Form.Text className="text-danger">{errors.name}<br/></Form.Text>}
          </Form.Group>
          { user.patronymic && 
          <Form.Group>
            <Form.Label>Отчество</Form.Label>
            <Form.Control
              type="text"
              value={formData.patronymic}
              onChange={(e) => setFormData({ ...formData, patronymic: e.target.value })}
            />
          </Form.Group>}
          <Form.Group>
            <Form.Label>Должность</Form.Label>
            <Form.Control
              type="text"
              value={formData.position}
              onChange={(e) => handleInputChange("position", e.target.value)}
              isInvalid={!!errors.position}
            />
            {errors.position && <Form.Text className="text-danger">{errors.position}<br/></Form.Text>}
          </Form.Group>
          <Form.Group>
            <Form.Label>Email</Form.Label>
            <Form.Control
              type="email"
              value={formData.email}
              onChange={(e) => handleInputChange("email", e.target.value)}
              isInvalid={!!errors.email}
            />
            {errors.email && <Form.Text className="text-danger">{errors.email}<br/></Form.Text>}
          </Form.Group>  
      </Modal.Body>
      <Modal.Footer>
      <button className="apply-button" onClick={handleSave}>
            Сохранить
          </button>
          <button className="cansel-button" onClick={canselEding}>
            Отменить
          </button>
      </Modal.Footer>
    </Modal>

    <Modal show={showDeleteConfirm} onHide={cancelDeleteClick}>
      <Modal.Header closeButton>
        <Modal.Title>Подтвердите удаление</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        Вы уверены, что хотите удалить ваш профиль?
      </Modal.Body>
      <Modal.Footer>
        <button className="reset-button" onClick={confirmDeleteClick}>
          Удалить
        </button>
        <button className="cansel-button" onClick={cancelDeleteClick}>
          Отмена
        </button>
      </Modal.Footer>
    </Modal>

    <Modal show={showEditPassword} onHide={cancelEditPassword}>
      <Modal.Header closeButton>
        <Modal.Title>Изменение пароля</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <p>На почту {user.email} отправлен код подтверждения.</p>
        <Form>
        <Form.Group className="sign-left-group">
            <Form.Label>Код подтверждения</Form.Label>
            <Form.Control
              className="control-input"
              type="text"
              value={passwordForm.code}
              onChange={(e) => setPasswordForm({ ...passwordForm, code: e.target.value })}
              isInvalid={!!errors.code}
            />
            {errors.code && <Form.Text className="text-danger">{errors.code}<br/></Form.Text>}
          </Form.Group>
          <Form.Group className="password-group sign-left-group">
            <Form.Label>Новый пароль</Form.Label>
            <Form.Control
              className="control-input"
              type={showPassword ? "text" : "password"}
              value={passwordForm.password}
              onChange={(e) => setPasswordForm({ ...passwordForm, password: e.target.value })}
              isInvalid={!!errors.password}
            />
            {errors.password && <Form.Text className="text-danger">{errors.password}<br/></Form.Text>}

            {showPasswordEyes && (showPassword ? (
              <EyeOff className="eye-icon" onClick={() => setShowPassword(false)} />
            ) : (
              <Eye className="eye-icon" onClick={() => setShowPassword(true)} />
            ))}
          </Form.Group>
          <Form.Group className="password-group sign-left-group">
            <Form.Label>Повторите пароль</Form.Label>
            <Form.Control
              className="control-input"
              type={showConfirmPassword ? "text" : "password"}
              value={passwordForm.confirmPassword}
              onChange={(e) => setPasswordForm({ ...passwordForm, confirmPassword: e.target.value })}
              isInvalid={!!errors.confirmPassword}
            />
            {errors.confirmPassword && <Form.Text className="text-danger">{errors.confirmPassword}<br/></Form.Text>}

            {showConfirmPasswordEyes && (showConfirmPassword ? (
              <EyeOff className="eye-icon" onClick={() => setShowConfirmPassword(false)} />
            ) : (
              <Eye className="eye-icon" onClick={() => setShowConfirmPassword(true)} />
            ))}
          </Form.Group>
        </Form>
      </Modal.Body>
      <Modal.Footer>
        <button className="apply-button" onClick={confirmEditPasswordClick}>
          Изменить
        </button>
        <button className="cansel-button" onClick={cancelEditPassword}>
          Отмена
        </button>
      </Modal.Footer>
    </Modal>
    </div>
  );
};

export default ProfilePage;

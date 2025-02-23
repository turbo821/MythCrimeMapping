import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Eye, EyeOff } from "lucide-react";
import { Modal, Form } from "react-bootstrap";
import api from "../api";
import { getUserId, removeToken } from "../services/authFunctions";
import "./ProfilePage.css";

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

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSave = async () => {
    try {
      await api.patch(`/api/users/${id}`, formData, {
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
    setIsEditing(true);
  }

  const canselEding = () => {
    setIsEditing(false);
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

  const handleEditPassword = () => {
    setEditPassword(true);
  }

  const cancelEditPassword = () => {
    setEditPassword(false);
  }

  const confirmEditPasswordClick = () => {

  }

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
            <p><strong>Фамилия:</strong> {user.name}</p>
            <p><strong>Имя:</strong> {user.surname}</p>
            <p><strong>Отчество:</strong> {user.patronymic}</p>
            <p><strong>Должность:</strong> {user.position}</p>
            <p><strong>Email:</strong> {user.email}</p>
          </div>
        </div>


        {currentUserId === id && 
          <div class="profile-button-group">
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
            />
          </Form.Group>
          <Form.Group>
            <Form.Label>Имя</Form.Label>
            <Form.Control
              type="text"
              value={formData.name}
              onChange={(e) => handleInputChange("name", e.target.value)}
            />
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
            />
          </Form.Group>
          <Form.Group>
            <Form.Label>Email</Form.Label>
            <Form.Control
              type="email"
              value={formData.email}
              onChange={(e) => handleInputChange("email", e.target.value)}
            />
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
              onChange={(e) => setPasswordForm({ ...formData, code: e.target.value })}
            />
          </Form.Group>
          <Form.Group className="password-group sign-left-group">
            <Form.Label>Новый пароль</Form.Label>
            <Form.Control
              className="control-input"
              type={showPassword ? "text" : "password"}
              value={passwordForm.password}
              onChange={(e) => setPasswordForm({ ...formData, password: e.target.value })}
            />
            {showPassword ? (
              <EyeOff className="eye-icon" onClick={() => setShowPassword(false)} />
            ) : (
              <Eye className="eye-icon" onClick={() => setShowPassword(true)} />
            )}
          </Form.Group>
          <Form.Group className="password-group sign-left-group">
            <Form.Label>Повторите пароль</Form.Label>
            <Form.Control
              className="control-input"
              type={showConfirmPassword ? "text" : "password"}
              value={passwordForm.confirmPassword}
              onChange={(e) => setPasswordForm({ ...formData, confirmPassword: e.target.value })}
            />
            {showConfirmPassword ? (
              <EyeOff className="eye-icon" onClick={() => setShowConfirmPassword(false)} />
            ) : (
              <Eye className="eye-icon" onClick={() => setShowConfirmPassword(true)} />
            )}
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

function setToken(token) {
  localStorage.setItem("token", token);
};

function getToken() {
  return localStorage.getItem("token");
};

function getUserId() {
  const token = localStorage.getItem("token");
  if(token === null) {
    return null;
  }
  const decoded = JSON.parse(atob(token.split(".")[1]));
  return decoded.nameid;
};

function removeToken() {
  localStorage.removeItem("token");
};

export { setToken, getUserId, getToken, removeToken };
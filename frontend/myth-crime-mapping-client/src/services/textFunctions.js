
// string processing
const capitalizeFirstLetter = (str) => {
    if (typeof str !== 'string' || str.length === 0) return null;
    return str.charAt(0).toLocaleUpperCase() + str.slice(1).toLocaleLowerCase();
  }

function isValidEmail(email) {
    const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailPattern.test(email);
}

export { capitalizeFirstLetter, isValidEmail };
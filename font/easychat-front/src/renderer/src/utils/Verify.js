const regs = {
  email: /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/,
  number: /^\+?[1-9][0-9]*$/,
  password: /^(?=.*\d)(?=.*[a-zA-Z])[\da-zA-Z~!@#$%^&*_./]{8,}/,
  version: /^[0-9.*]+$/
}

const verify = (rule, value, reg, callback) => {
  if (value) {
    if (reg.test(value)) {
      callback()
    } else {
      callback(new Error(rule.message))
    }
  } else {
    callback()
  }
}

const checkPassword = (value) => {
  return regs.password.test(value)
}

const checkEmail = (value) => {
  return regs.email.test(value)
}

const password = (rule, value, callback) => {
  return verify(rule, value, regs.password, callback)
}

const number = (rule, value, callback) => {
  return verify(rule, value, regs.number, callback)
}

const version = (rule, value, callback) => {
  return verify(rule, value, regs.version, callback)
}

export default {
  checkPassword,
  checkEmail,
  password,
  number,
  version
}

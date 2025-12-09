<template>
  <div class="auth-container">
    <h1 class="title">{{ isRegisterMode ? 'Skapa konto' : 'Logga in' }}</h1>

    <!-- Toggle mellan registrera / logga in -->
    <div class="toggle">
      <button
        :class="['toggle-btn', { active: isRegisterMode }]"
        @click="isRegisterMode = true"
      >
        Registrera
      </button>
      <button
        :class="['toggle-btn', { active: !isRegisterMode }]"
        @click="isRegisterMode = false"
      >
        Logga in
      </button>
    </div>

    <!-- Registrera -->
    <form v-if="isRegisterMode" @submit.prevent="onRegister" class="card">
      <h2>Skapa konto</h2>
      <input v-model="registerName" placeholder="Namn" required class="input" />
      <input v-model="registerEmail" type="email" placeholder="Email" required class="input" />
      <input v-model="registerPassword" type="password" placeholder="Lösenord" required class="input" />

      <select v-model="role" class="input">
        <option value="user">User</option>
        <option value="admin">Admin</option>
      </select>

      <!-- Admin Secret Field -->
      <input
        v-if="role === 'admin'"
        v-model="adminSecret"
        type="password"
        placeholder="Hemlig admin-kod (3 siffror)"
        class="input"
        required
      />

      <button type="submit" class="primary-btn">Skapa konto</button>
    </form>

    <!-- Logga in -->
    <form v-else @submit.prevent="onLogin" class="card">
      <h2>Logga in</h2>
      <input v-model="loginEmail" type="email" placeholder="Email" required class="input" />
      <input v-model="loginPassword" type="password" placeholder="Lösenord" required class="input" />
      <button type="submit" class="primary-btn">Logga in</button>
    </form>

    <p v-if="error" class="error">{{ error }}</p>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'Login',
  data() {
    return {
      isRegisterMode: true,
      loginEmail: '',
      loginPassword: '',
      registerName: '',
      registerEmail: '',
      registerPassword: '',
      role: 'user',
      adminSecret: '',
      error: '',
    };
  },
  methods: {
    async onLogin() {
      this.error = '';
      try {
        const response = await axios.post('http://localhost:5000/api/users/login', {
          email: this.loginEmail,
          password: this.loginPassword,
        });

        const user = response.data;
        if (user.isAdmin) {
          this.$router.push('/admin-tickets');
        } else {
          this.$router.push('/user-tickets');
        }
      } catch (err) {
        this.error = 'Fel email eller lösenord.';
        console.error(err);
      }
    },

    async onRegister() {
      this.error = '';
      try {
        const isAdmin = this.role === 'admin';
        await axios.post('http://localhost:5000/api/users/register', {
          name: this.registerName,
          email: this.registerEmail,
          password: this.registerPassword,
          isAdmin,
          adminSecret: this.adminSecret,
        });

        // enkel feedback: auto-fyll loginfält och växla till login
        this.loginEmail = this.registerEmail;
        this.loginPassword = this.registerPassword;
        this.registerName = '';
        this.registerEmail = '';
        this.registerPassword = '';
        this.role = 'user';
        this.adminSecret = '';
      } catch (err) {
        this.error = 'Kunde inte skapa konto (kolla admin-kod eller om email redan används).';
        console.error(err);
      }
    },
  },
};
</script>

<style scoped>
.auth-container {
  max-width: 420px;
  margin: 60px auto;
  text-align: center;
  font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
}

.title {
  margin-bottom: 24px;
  font-size: 1.8rem;
}

.toggle {
  display: flex;
  justify-content: center;
  margin-bottom: 20px;
  border-radius: 999px;
  overflow: hidden;
  border: 1px solid #d0d0d0;
}

.toggle-btn {
  flex: 1;
  padding: 8px 12px;
  border: none;
  background: #f5f5f5;
  cursor: pointer;
  font-size: 0.95rem;
}

.toggle-btn.active {
  background: #2563eb;
  color: #fff;
}

.card {
  display: flex;
  flex-direction: column;
  gap: 10px;
  padding: 20px 18px 18px;
  border-radius: 8px;
  background: #ffffff;
  box-shadow: 0 8px 20px rgba(15, 23, 42, 0.12);
}

.card h2 {
  margin-bottom: 4px;
}

.input {
  padding: 8px 10px;
  border-radius: 6px;
  border: 1px solid #d0d0d0;
  font-size: 0.95rem;
}

.input:focus {
  outline: none;
  border-color: #2563eb;
  box-shadow: 0 0 0 1px #2563eb33;
}

.primary-btn {
  padding: 9px 12px;
  border-radius: 6px;
  border: none;
  background: #2563eb;
  color: #fff;
  cursor: pointer;
  font-weight: 500;
}

.primary-btn:hover {
  background: #1d4ed8;
}

.error {
  margin-top: 14px;
  color: #b91c1c;
}
</style>
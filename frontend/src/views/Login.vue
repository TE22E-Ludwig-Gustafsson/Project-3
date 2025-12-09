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
  max-width: 400px;
  margin: 40px auto;
  text-align: center;
  font-family: Arial, sans-serif;
}

.title {
  margin-bottom: 20px;
}

.toggle {
  display: flex;
  justify-content: center;
  margin-bottom: 20px;
}

.toggle-btn {
  flex: 1;
  padding: 8px 12px;
  border: 1px solid #ccc;
  background: #f5f5f5;
  cursor: pointer;
}

.toggle-btn.active {
  background: #007bff;
  color: #fff;
}

.card {
  display: flex;
  flex-direction: column;
  gap: 10px;
  padding: 16px;
  border: 1px solid #ddd;
  border-radius: 4px;
  background: #fff;
}

.input {
  padding: 8px;
  border-radius: 4px;
  border: 1px solid #ccc;
}

.primary-btn {
  padding: 10px;
  border-radius: 4px;
  border: none;
  background: #007bff;
  color: #fff;
  cursor: pointer;
}

.primary-btn:hover {
  background: #0069d9;
}

.error {
  margin-top: 12px;
  color: red;
}
</style>
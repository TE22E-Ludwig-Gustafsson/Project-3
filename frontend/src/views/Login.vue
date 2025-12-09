<template>
  <div>
    <h1>Login / Skapa konto</h1>

    <form @submit.prevent="onLogin">
      <h2>Logga in</h2>
      <input v-model="loginEmail" type="email" placeholder="Email" required />
      <input v-model="loginPassword" type="password" placeholder="Lösenord" required />
      <button type="submit">Logga in</button>
    </form>

    <hr />

    <form @submit.prevent="onRegister">
      <h2>Skapa konto</h2>
      <input v-model="registerName" placeholder="Namn" required />
      <input v-model="registerEmail" type="email" placeholder="Email" required />
      <input v-model="registerPassword" type="password" placeholder="Lösenord" required />

      <select v-model="role">
        <option value="user">User</option>
        <option value="admin">Admin</option>
      </select>

      <!-- Admin Secret Field -->
      <input
        v-if="role === 'admin'"
        v-model="adminSecret"
        type="password"
        placeholder="Hemlig admin-kod (3 siffror)"
        required
      />

      <button type="submit">Skapa konto</button>
    </form>

    <p v-if="error" style="color:red">{{ error }}</p>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'Login',
  data() {
    return {
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

</style>
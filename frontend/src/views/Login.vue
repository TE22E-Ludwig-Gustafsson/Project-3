<template>
  <div>
    <h1>Login</h1>
    <form @submit.prevent="login">
      <input v-model="username" placeholder="Username" />
      <input v-model="password" type="password" placeholder="Password" />
      <button type="submit">Login</button>
    </form>
  </div>
</template>

<script>
export default {
  name: 'Login',
  data() {
    return {
      username: '',
      password: '',
    };
  },
  methods: {
    login() {
      // Kontrollera roll och navigera till rätt sida
      if (this.username === 'admin') {
        this.$router.push('/admin-tickets');
      } else {
        this.$router.push('/user-tickets');
      }
    },
  },
};
</script>

<style scoped>

</style>
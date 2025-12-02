<template>
  <div>
    <h1>User Tickets</h1>
    <form @submit.prevent="createTicket">
      <input v-model="title" placeholder="Title" />
      <textarea v-model="description" placeholder="Description"></textarea>
      <button type="submit">Create Ticket</button>
    </form>
    <ul>
      <li v-for="ticket in tickets" :key="ticket.id">
        {{ ticket.title }} - {{ ticket.description }} - {{ ticket.status }}
      </li>
    </ul>
  </div>
</template>

<script>
export default {
  name: 'UserTickets',
  data() {
    return {
      title: '',
      description: '',
      tickets: [],
    };
  },
  methods: {
    createTicket() {
      // Logik för att skapa ett nytt ärende
      const newTicket = {
        id: this.tickets.length + 1,
        title: this.title,
        description: this.description,
        status: 'Open',
      };
      this.tickets.push(newTicket);
      this.title = '';
      this.description = '';
    },
  },
};
</script>

<style scoped>
/* Add styles here */
</style>
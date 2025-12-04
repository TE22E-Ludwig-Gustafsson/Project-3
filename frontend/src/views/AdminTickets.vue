<template>
  <div>
    <h1>Admin Tickets</h1>
    <ul>
      <li v-for="ticket in tickets" :key="ticket.id">
        {{ ticket.title }} - {{ ticket.description }} - {{ ticket.status }}
        <select v-model="ticket.status">
          <option>öppen</option>
          <option>Pågående</option>
          <option>Löst</option>
          <option>Stängd</option>
        </select>
        <button @click="updateStatus(ticket)">updatera status</button>
      </li>
    </ul>
  </div>
</template>

<script>
export default {
  name: 'AdminTickets',
  data() {
    return {
      tickets: [
        { id: 1, title: 'ärende 1', description: 'Beskrivning 1', status: 'öppen' },
        { id: 2, title: 'ärende 2', description: 'Beskrivning 2', status: 'Pågående' },
      ],
    };
  },
  methods: {
    updateStatus(ticket) {
      // Logik för att uppdatera status
      console.log(`Uppdaterade ärende ${ticket.id} till status ${ticket.status}`);
    },
  },
};
</script>

<style scoped>
/* Add styles here */
</style>
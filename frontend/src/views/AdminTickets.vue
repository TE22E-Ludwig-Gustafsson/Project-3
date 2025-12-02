<template>
  <div>
    <h1>Admin Tickets</h1>
    <ul>
      <li v-for="ticket in tickets" :key="ticket.id">
        {{ ticket.title }} - {{ ticket.description }} - {{ ticket.status }}
        <select v-model="ticket.status">
          <option>Open</option>
          <option>In Progress</option>
          <option>Resolved</option>
          <option>Closed</option>
        </select>
        <button @click="updateStatus(ticket)">Update Status</button>
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
        { id: 1, title: 'Issue 1', description: 'Description 1', status: 'Open' },
        { id: 2, title: 'Issue 2', description: 'Description 2', status: 'In Progress' },
      ],
    };
  },
  methods: {
    updateStatus(ticket) {
      // Logik för att uppdatera status
      console.log(`Updated ticket ${ticket.id} to status ${ticket.status}`);
    },
  },
};
</script>

<style scoped>
/* Add styles here */
</style>
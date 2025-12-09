<template>
  <div>
    <h1>Admin Tickets</h1>
    <ul>
      <li v-for="ticket in tickets" :key="ticket.id">
        {{ ticket.title }} - {{ ticket.description }} - {{ ticket.statusName }}
        <select v-model.number="ticket.statusId">
          <option v-for="s in statuses" :key="s.id" :value="s.id">
            {{ s.name }}
          </option>
        </select>
        <button @click="updateStatus(ticket)">uppdatera status</button>
        <button @click="deleteTicket(ticket)" style="margin-left: 8px; color: red;">
          ta bort
        </button>
      </li>
    </ul>
    <p v-if="error" style="color:red">{{ error }}</p>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'AdminTickets',
  data() {
    return {
      tickets: [],
      statuses: [
        { id: 1, name: 'Öppen' },
        { id: 2, name: 'Pågående' },
        { id: 3, name: 'Löst' },
        { id: 4, name: 'Stängd' },
      ],
      error: '',
    };
  },
  created() {
    this.loadTickets();
  },
  methods: {
    async loadTickets() {
      this.error = '';
      try {
        const response = await axios.get('http://localhost:5000/api/tickets');
        this.tickets = response.data.map(t => ({
          id: t.id,
          title: t.title,
          description: t.description,
          statusId: t.statusId,
          statusName: this.getStatusName(t.statusId),
        }));
      } catch (err) {
        this.error = 'Kunde inte hämta ärenden.';
        console.error(err);
      }
    },
    getStatusName(id) {
      const s = this.statuses.find(x => x.id === id);
      return s ? s.name : '';
    },
    async updateStatus(ticket) {
      this.error = '';
      try {
        await axios.put(
          `http://localhost:5000/api/tickets/${ticket.id}/status`,
          { statusId: ticket.statusId } // skicka som objekt
        );
        ticket.statusName = this.getStatusName(ticket.statusId);
      } catch (err) {
        this.error = 'Kunde inte uppdatera status.';
        console.error(err);
      }
    },
    async deleteTicket(ticket) {
      this.error = '';
      if (!confirm(`Ta bort ärende "${ticket.title}"?`)) {
        return;
      }
      try {
        await axios.delete(`http://localhost:5000/api/tickets/${ticket.id}`);
        this.tickets = this.tickets.filter(t => t.id !== ticket.id);
      } catch (err) {
        this.error = 'Kunde inte ta bort ärendet.';
        console.error(err);
      }
    },
  },
};
</script>

<style scoped>
/* Add styles here */
</style>
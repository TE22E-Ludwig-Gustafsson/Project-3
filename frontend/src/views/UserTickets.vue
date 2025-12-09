<template>
  <div>
    <h1>User Tickets</h1>
    <form @submit.prevent="createTicket">
      <input v-model="title" placeholder="Titel" />
      <textarea v-model="description" placeholder="Beskrivning"></textarea>
      <button type="submit" :disabled="saving">
        {{ saving ? 'Sparar...' : 'Skapa ärende' }}
      </button>
    </form>
    <p v-if="error" style="color: red">{{ error }}</p>
    <ul>
      <li v-for="ticket in tickets" :key="ticket.id">
        {{ ticket.title }} - {{ ticket.description }} - {{ ticket.statusName }}
      </li>
    </ul>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'UserTickets',
  data() {
    return {
      title: '',
      description: '',
      tickets: [],
      saving: false,
      error: '',
      currentUserId: 2, 
      defaultStatusId: 1, 
    };
  },
  created() {
    this.loadTickets();
  },
  methods: {
    async loadTickets() {
      try {
        const response = await axios.get('http://localhost:5000/api/tickets');
        // Anpassa till backend-modellen (Status.Name)
        this.tickets = response.data.map(t => ({
          id: t.id,
          title: t.title,
          description: t.description,
          statusName: t.status ? t.status.name : '',
        }));
      } catch (err) {
        this.error = 'Kunde inte hämta ärenden.';
        console.error(err);
      }
    },
    async createTicket() {
      this.error = '';
      if (!this.title) {
        this.error = 'Titel krävs.';
        return;
      }
      this.saving = true;
      try {
        await axios.post('http://localhost:5000/api/tickets', {
          title: this.title,
          description: this.description,
          userId: this.currentUserId,
          statusId: this.defaultStatusId,
        });
        this.title = '';
        this.description = '';
        await this.loadTickets();
      } catch (err) {
        this.error = 'Kunde inte spara ärendet.';
        console.error(err);
      } finally {
        this.saving = false;
      }
    },
  },
};
</script>

<style scoped>
/* Add styles here */
</style>
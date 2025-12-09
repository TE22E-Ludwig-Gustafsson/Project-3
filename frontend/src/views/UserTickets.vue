<template>
  <div class="user-page">
    <div class="ticket-form-card">
      <h1>Mina ärenden</h1>
      <form @submit.prevent="createTicket" class="form-grid">
        <input v-model="title" class="input" placeholder="Titel" />
        <textarea v-model="description" class="input textarea" placeholder="Beskrivning"></textarea>
        <button type="submit" class="primary-btn" :disabled="saving">
          {{ saving ? 'Sparar...' : 'Skapa ärende' }}
        </button>
      </form>
      <p v-if="error" class="error">{{ error }}</p>
    </div>

    <div class="tickets-list-card">
      <h2>Alla ärenden</h2>
      <ul v-if="tickets.length" class="ticket-list">
        <li v-for="ticket in tickets" :key="ticket.id" class="ticket-item">
          <div class="ticket-header">
            <span class="ticket-title">{{ ticket.title }}</span>
            <span class="ticket-status">{{ ticket.statusName }}</span>
          </div>
          <p class="ticket-description">{{ ticket.description }}</p>
        </li>
      </ul>
      <p v-else class="empty">Inga ärenden ännu.</p>
    </div>
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
      statuses: [
        { id: 1, name: 'Öppen' },
        { id: 2, name: 'Pågående' },
        { id: 3, name: 'Löst' },
        { id: 4, name: 'Stängd' },
      ],
    };
  },
  created() {
    this.loadTickets();
  },
  methods: {
    async loadTickets() {
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
    getStatusName(id) {
      const s = this.statuses.find(x => x.id === id);
      return s ? s.name : '';
    },
  },
};
</script>

<style scoped>
.user-page {
  max-width: 900px;
  margin: 40px auto;
  display: grid;
  grid-template-columns: 1.1fr 1.2fr;
  gap: 24px;
  font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
}

.ticket-form-card,
.tickets-list-card {
  background: #ffffff;
  border-radius: 10px;
  padding: 18px 18px 16px;
  box-shadow: 0 8px 20px rgba(15, 23, 42, 0.08);
}

.ticket-form-card h1 {
  margin-bottom: 14px;
}

.tickets-list-card h2 {
  margin-bottom: 10px;
}

.form-grid {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.input {
  padding: 8px 10px;
  border-radius: 6px;
  border: 1px solid #d0d0d0;
  font-size: 0.95rem;
}

.textarea {
  min-height: 80px;
  resize: vertical;
}

.input:focus {
  outline: none;
  border-color: #2563eb;
  box-shadow: 0 0 0 1px #2563eb33;
}

.primary-btn {
  align-self: flex-start;
  padding: 8px 14px;
  border-radius: 6px;
  border: none;
  background: #16a34a;
  color: #fff;
  cursor: pointer;
  font-weight: 500;
}

.primary-btn:disabled {
  opacity: 0.7;
  cursor: default;
}

.primary-btn:not(:disabled):hover {
  background: #15803d;
}

.error {
  margin-top: 8px;
  color: #b91c1c;
}

.ticket-list {
  list-style: none;
  margin: 0;
  padding: 0;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.ticket-item {
  padding: 10px 11px;
  border-radius: 8px;
  border: 1px solid #e5e7eb;
  background: #f9fafb;
}

.ticket-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 4px;
}

.ticket-title {
  font-weight: 600;
}

.ticket-status {
  font-size: 0.85rem;
  padding: 2px 8px;
  border-radius: 999px;
  background: #e0f2fe;
  color: #0369a1;
}

.ticket-description {
  margin: 0;
  font-size: 0.9rem;
  color: #4b5563;
}

.empty {
  margin-top: 6px;
  font-size: 0.9rem;
  color: #6b7280;
}

@media (max-width: 800px) {
  .user-page {
    grid-template-columns: 1fr;
  }
}
</style>
<template>
  <div>
    <img src="@/assets/arrow.png" alt="image" style="width: 100px" />
    <b style="font-size: 30px"> Resumen de Inversiones</b>
    <img src="@/assets/arrow.png" alt="image" style="width: 100px" />
  </div>
  <div class="component">
    <div class="investments">
      <div>Gastos por compras y ventas: {{ calculateInvestment() }}</div>
      <div>Poseciones: {{ totalCash }}</div>
      <div>Total Ganancias/Perdidas= {{ calculateTotalProfit() }}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";

export default {
  data() {
    return {
      transactions: [],
      money: 0,
    };
  },
  computed: {
    ...mapGetters(["userId"]),
    ...mapGetters("transactions", ["getWallet"]),
    ...mapGetters(["getBTCPrice", "getETHPrice", "getUSDTPrice"]),
    totalCash() {
      let total = 0;
      for (let cryptoCode in this.getWallet) {
        total += this.calculateCash(this.getWallet[cryptoCode], cryptoCode);
      }
      return total;
    },
  },
  methods: {
    ...mapActions("transactions", ["getState"]),
    ...mapActions("transactions", ["getHistory"]),
    calculateInvestment() {
      let buys = 0;
      let sales = 0;

      for (let index = 0; index < this.transactions.length; index++) {
        let transaction = this.transactions[index];

        if (transaction.action === "purchase") {
          buys -= transaction.money;
        } else if (transaction.action === "sale") {
          sales += transaction.money;
        }
      }

      let invTotal = buys + sales;
      return invTotal;
    },
    calculateCash(amount, cryptoCode) {
      const code = cryptoCode.toUpperCase();
      const cryptoGetter = `get${code}Price`;
      const cryptoPrice = this[cryptoGetter];
      if (cryptoPrice) {
        const cash = parseFloat(amount * cryptoPrice.totalAsk);
        return cash;
      } else {
        console.error(`Getter ${cryptoGetter} no encontrado`);
        return 0;
      }
    },
    calculateTotalProfit() {
      let totalInvestments = this.calculateInvestment();
      let totalCashValue = this.totalCash;

      let totalProfit = totalInvestments + totalCashValue;
      return totalProfit.toFixed(2);
    },
    async fetchData() {
      try {
        await this.getState(this.userId);
      } catch (error) {
        console.error("Error:", error);
      }
    },
  },
  async created() {
    try {
      this.transactions = await this.getHistory(this.userId);
      this.fetchData();
    } catch (error) {
      console.error("Error al obtener el historial:", error);
    }
  },
};
</script>

<style scoped>
.component {
  display: flex;
  justify-content: center;
  align-items: center;
}

.investments {
  width: 50%;
  border: 1px solid #35314a;
  border-radius: 15px;
  padding: 15px;
}
</style>

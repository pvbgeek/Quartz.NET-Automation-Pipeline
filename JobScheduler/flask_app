from flask import Flask, request, jsonify
import matplotlib
matplotlib.use('Agg')  # Use a non-GUI backend for Matplotlib
import matplotlib.pyplot as plt
import os

app = Flask(__name__)

# Folder to save plots
PLOT_DIR = os.path.join(os.getcwd(), "JobScheduler", "plots")
os.makedirs(PLOT_DIR, exist_ok=True)

@app.route("/plot/aggregated-sales", methods=["POST"])
def plot_aggregated_sales():
    try:
        data = request.get_json()
        if not data:
            return jsonify({"error": "No data provided"}), 400

        # Extract data
        regions = [item["Region"] for item in data]
        total_sales = [item["TotalSales"] for item in data]

        # Bar chart (1.png)
        plt.figure(figsize=(10, 6))
        plt.bar(regions, total_sales, color='blue')
        plt.xlabel("Region")
        plt.ylabel("Total Sales")
        plt.title("Aggregated Sales by Region")
        plt.xticks(rotation=45)
        plt.tight_layout()
        plt.savefig(os.path.join(PLOT_DIR, "1.png"))
        plt.close()

        # Pie chart (2.png)
        plt.figure(figsize=(8, 8))
        plt.pie(total_sales, labels=regions, autopct='%1.1f%%', startangle=90, colors=plt.cm.Paired.colors)
        plt.title("Aggregated Sales Distribution by Region")
        plt.axis('equal')  # Equal aspect ratio ensures the pie chart is circular.
        plt.savefig(os.path.join(PLOT_DIR, "2.png"))
        plt.close()

        return jsonify({"message": "Aggregated Sales plots created", "bar_chart": "1.png", "pie_chart": "2.png"}), 200
    except Exception as e:
        return jsonify({"error": str(e)}), 500

@app.route("/plot/monthly-trends", methods=["POST"])
def plot_monthly_trends():
    try:
        data = request.get_json()
        if not data:
            return jsonify({"error": "No data provided"}), 400

        # Extract data
        months = [item["Month"] for item in data]
        sales_growth = [item["SalesGrowth"] for item in data]
        categories = list(set(item["Category"] for item in data))

        # Line chart (3.png)
        plt.figure(figsize=(12, 7))
        for category in categories:
            category_data = [item for item in data if item["Category"] == category]
            category_months = [item["Month"] for item in category_data]
            category_sales_growth = [item["SalesGrowth"] for item in category_data]
            plt.plot(category_months, category_sales_growth, label=category)
        plt.xlabel("Month")
        plt.ylabel("Sales Growth")
        plt.title("Monthly Sales Trends")
        plt.legend()
        plt.grid(True)
        plt.tight_layout()
        plt.savefig(os.path.join(PLOT_DIR, "3.png"))
        plt.close()

        return jsonify({"message": "Monthly Trends plots created", "line_chart": "3.png"}), 200
    except Exception as e:
        return jsonify({"error": str(e)}), 500

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)
